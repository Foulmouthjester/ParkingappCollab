using Microsoft.AspNetCore.Mvc;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Backend.Services;



namespace Parkingapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly ParkingDbContext _context;
        private readonly ParkingCostService _costService;

        public ParkingController(ParkingDbContext context, ParkingCostService costService)
        {
            _context = context;
            _costService = costService;
        }

        // DTOs
        public class RegisterRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CarDto
        {
            public string Name { get; set; }
        }

        public class StartParkingRequest
        {
            public int CarId { get; set; }
        }

        public class EndParkingRequest
        {
            public int SessionId { get; set; }
        }

        // Register endpoint (POST /api/auth/register)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            // Validate the model (thanks to [Required] annotations)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check for existing user
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            // Create new user
            var newUser = new User
            {
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),                
            };

            // Add and save to database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful!", UserId = newUser.Id });
        }

        // Login endpoint (POST /api/auth/login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            // For now, just return success; we can add a token later
            return Ok(new { Message = "Login successful!", UserId = user.Id });
        }

        // GET: api/auth/cars (Get all cars for the logged-in user)
        [HttpGet("cars")]
        public IActionResult GetCars()
        {
            Console.WriteLine("GetCars called");
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("User not authenticated");
            var cars = _context.Cars.Where(c => c.UserId == userId.Value).ToList();
            Console.WriteLine($"Fetched {cars.Count} cars for UserId {userId}");
            return Ok(cars);
        }

        // POST: api/auth/cars (Register a new car)
        [HttpPost("cars")]
        public IActionResult RegisterCar([FromBody] CarDto carDto)
        {
            Console.WriteLine($"RegisterCar called with Name: {carDto?.Name}");
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("User not authenticated");
            if (string.IsNullOrWhiteSpace(carDto?.Name)) return BadRequest(new { error = "Car name is required" });

            var newCar = new Car
            {
                Name = carDto.Name.Trim(),
                Cost = 0,
                UserId = userId.Value
            };

            _context.Cars.Add(newCar);
            _context.SaveChanges();

            Console.WriteLine($"Car saved: Id={newCar.Id}, Name={newCar.Name}, UserId={newCar.UserId}");
            return CreatedAtAction(nameof(GetCars), new { id = newCar.Id }, newCar);
        }

        // DELETE: api/auth/cars/{id}
        [HttpDelete("cars/{id}")]
        public IActionResult RemoveCar(int id)
        {
            Console.WriteLine($"RemoveCar called for Id: {id}");
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("User not authenticated");

            var car = _context.Cars.FirstOrDefault(c => c.Id == id && c.UserId == userId.Value);
            if (car == null) return NotFound("Car not found or not owned by user");

            _context.Cars.Remove(car);
            _context.SaveChanges();

            Console.WriteLine($"Car removed: Id={id}");
            return NoContent();
        }

        [HttpPost("start-parking")]
        public async Task<IActionResult> StartParking([FromQuery] int userId, [FromBody] StartParkingRequest request)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == request.CarId && c.UserId == userId);
            if (car == null) return NotFound("Car not found or does not belong to user.");
            var session = new ParkingSession
            {
                CarId = request.CarId,
                StartTime = DateTime.UtcNow
            };
            _context.ParkingSessions.Add(session);
            await _context.SaveChangesAsync();
            return Ok(new { SessionId = session.Id });
        }

        [HttpPost("end-parking")]
        public async Task<IActionResult> EndParking([FromQuery] int userId, [FromBody] EndParkingRequest request)
        {
            Console.WriteLine($"Ending parking for userId={userId}, sessionId={request.SessionId}");
            var session = await _context.ParkingSessions
                .Include(s => s.Car)
                .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.Car.UserId == userId);
            if (session == null)
            {
                Console.WriteLine("Session not found or doesn’t belong to user.");
                return NotFound("Parking session not found or does not belong to user.");
            }
            if (session.EndTime.HasValue)
            {
                Console.WriteLine("Session already ended.");
                return BadRequest("Parking session already ended.");
            }

            session.EndTime = DateTime.UtcNow;
            session.Cost = _costService.CalculateCost(session.StartTime, session.EndTime);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Parking ended. Cost: {session.Cost}");
            return Ok(new { cost = session.Cost }); 
        }
            


         [HttpGet("users/{userId}/total-cost")]
        public async Task<IActionResult> GetUserTotalCost(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Cars)
                .ThenInclude(c => c.ParkingSessions)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            var totalCost = user.Cars
                .SelectMany(c => c.ParkingSessions) // Flatten all ParkingSessions from all Cars
                .Sum(ps => ps.Cost ?? 0m);         // Sum the Costs, treating null as 0
            return Ok(new { TotalCost = totalCost });
        }

        private int? GetCurrentUserId()
        {
            var queryUserId = HttpContext.Request.Query["userId"].FirstOrDefault();
            Console.WriteLine($"Query userId from request: '{queryUserId}'");
            var userId = queryUserId ?? "1";
            return int.TryParse(userId, out int id) ? id : null;
        }
    }
}

