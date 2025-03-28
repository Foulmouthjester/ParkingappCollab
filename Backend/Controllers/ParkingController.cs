using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using System.Security.Cryptography;
using System.Text;
using ParkingSystem.DTOs;
using System.Linq;
using System;


namespace ParkingSystem.Controllers
{
  [ApiController]
  [Route("api/auth")]

  public class AuthController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
      _context = context;
    }

    // Register endpoint (POST /api/auth/register)
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
      var existingUser = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
      if (existingUser != null)
      {
        return BadRequest("User already exists");
      }

      var newUser = new User
      {
        Email = userDto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
        AccountBalance = 0
      };

      _context.Users.Add(newUser);
      _context.SaveChanges();

      return Ok(new { Message = "Registration successful!" });
    }

    // Login endpoint (POST /api/auth/login)
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
      var user = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
      if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
      {
        return Unauthorized("Invalid credentials");
      }

      // Generate JWT token or return success message
      return Ok(new { Message = "Login successful!" });
    }
    [HttpGet("cars")]
        public IActionResult GetCars()
        {
            Console.WriteLine("GetCars called");
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("User not authenticated");
            var cars = _context.Cars.Where(c => c.UserId == userId.Value).ToList();
            return Ok(cars);
        }

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

            Console.WriteLine($"Car saved: Id={newCar.Id}, Name={newCar.Name}");
            return CreatedAtAction(nameof(GetCars), new { id = newCar.Id }, newCar);
        }

        private int? GetCurrentUserId()
        {
            return 1; // Hardcoded for now
        }
  }
}

