using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using System.Security.Cryptography;
using System.Text;

namespace ParkingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController : ControllerBase
{
  private readonly ParkingDbContext _context;

  private const decimal DayRate = 14m;
  private const decimal NightRate = 6m;

  public ParkingController(ParkingDbContext context)
  {
    _context = context;
  }

  [HttpPost("register")]
  public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
  {
    if (await _context.Users.AnyAsync(u => u.Email == request.Email))
      return Conflict("Email is already registered.");

    var user = new User
    {
      Email = request.Email,
      PasswordHash = HashPassword(request.Password)
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok("User registered successfully.");
  }

  [HttpPost("register-car")]
  public async Task<IActionResult> RegisterCar([FromQuery] string email, [FromQuery] string licensePlate)
  {
    var user = await _context.Users.FindAsync(email);
    if (user == null)
      return NotFound("User not found.");

    if (user.Cars.Contains(licensePlate))
      return Conflict("Car is already registered.");

    user.Cars.Add(licensePlate);
    await _context.SaveChangesAsync();

    return Ok("Car registered successfully.");
  }

  [HttpPost("begin")]
  public async Task<IActionResult> BeginPeriod([FromQuery] string licensePlate)
  {
    if (await _context.ParkingPeriods.AnyAsync(p => p.LicensePlate == licensePlate))
      return Conflict("A parking period is already active for this car.");

    var period = new ParkingPeriod
    {
      LicensePlate = licensePlate,
      StartTime = DateTime.Now
    };

    _context.ParkingPeriods.Add(period);
    await _context.SaveChangesAsync();

    return Ok("Parking period started.");
  }

  [HttpPost("end")]
  public async Task<IActionResult> EndPeriod([FromQuery] string licensePlate)
  {
    var period = await _context.ParkingPeriods.FirstOrDefaultAsync(p => p.LicensePlate == licensePlate);
    if (period == null)
      return NotFound("No active parking period for this car.");

    _context.ParkingPeriods.Remove(period);
    await _context.SaveChangesAsync();

    var elapsedTime = DateTime.Now - period.StartTime;
    var cost = CalculateCost(period.StartTime, elapsedTime);

    var user = await _context.Users.FirstOrDefaultAsync(u => u.Cars.Contains(licensePlate));
    if (user != null)
    {
      user.AccountBalance += cost;
      await _context.SaveChangesAsync();
    }

    return Ok(new
    {
      LicensePlate = licensePlate,
      PeriodStart = period.StartTime,
      PeriodEnd = DateTime.Now,
      Cost = cost
    });
  }

  [HttpGet("account")]
  public async Task<IActionResult> GetAccountBalance([FromQuery] string email)
  {
    var user = await _context.Users.FindAsync(email);
    if (user == null)
      return NotFound("User not found.");

    return Ok(new { user.Email, user.AccountBalance });
  }

  private decimal CalculateCost(DateTime startTime, TimeSpan elapsedTime)
  {
    decimal cost = 0;
    var currentTime = startTime;

    while (elapsedTime.TotalHours > 0)
    {
      decimal hourlyRate = IsDayTime(currentTime) ? DayRate : NightRate;
      cost += hourlyRate;
      currentTime = currentTime.AddHours(1);
      elapsedTime -= TimeSpan.FromHours(1);
    }

    return cost;
  }

  private bool IsDayTime(DateTime time)
  {
    var hour = time.Hour;
    return hour >= 8 && hour < 18;
  }

  private string HashPassword(string password)
  {
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(password);
    var hash = sha256.ComputeHash(bytes);
    return Convert.ToBase64String(hash);
  }
}

