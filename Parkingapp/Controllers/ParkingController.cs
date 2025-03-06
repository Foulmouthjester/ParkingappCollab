
using Microsoft.AspNetCore.Mvc;
using ParkingSystem.Models;
using System.Globalization;

namespace ParkingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController : ControllerBase
{
  private static List<User> Users = new();
  private static List<ParkingPeriod> ActiveParkingPeriods = new();

  private const decimal DayRate = 14m; // SEK/hour 08:00-18:00
  private const decimal NightRate = 6m; // SEK/hour 18:00-08:00

  [HttpPost("register")]
  public IActionResult RegisterUser([FromBody] User user)
  {
    if (Users.Any(u => u.UserId == user.UserId))
      return Conflict("User already exists.");

    Users.Add(user);
    return Ok("User registered successfully.");
  }

  [HttpPost("begin")]
  public IActionResult BeginPeriod([FromQuery] string licensePlate)
  {
    if (ActiveParkingPeriods.Any(p => p.LicensePlate == licensePlate))
      return Conflict("A parking period is already active for this car.");

    ActiveParkingPeriods.Add(new ParkingPeriod
    {
      LicensePlate = licensePlate,
      StartTime = DateTime.Now
    });
    return Ok("Parking period started.");
  }

  [HttpPost("end")]
  public IActionResult EndPeriod([FromQuery] string licensePlate)
  {
    var period = ActiveParkingPeriods.FirstOrDefault(p => p.LicensePlate == licensePlate);
    if (period == null)
      return NotFound("No active parking period for this car.");

    ActiveParkingPeriods.Remove(period);

    var elapsedTime = DateTime.Now - period.StartTime;
    var cost = CalculateCost(period.StartTime, elapsedTime);

    var user = Users.FirstOrDefault(u => u.Cars.Contains(licensePlate));
    if (user != null)
      user.AccountBalance += cost;

    return Ok(new
    {
      LicensePlate = licensePlate,
      PeriodStart = period.StartTime,
      PeriodEnd = DateTime.Now,
      Cost = cost
    });
  }

  [HttpGet("present")]
  public IActionResult GetPresentPeriod([FromQuery] string licensePlate)
  {
    var period = ActiveParkingPeriods.FirstOrDefault(p => p.LicensePlate == licensePlate);
    if (period == null)
      return NotFound("No active parking period for this car.");

    return Ok(period);
  }

  [HttpGet("account")]
  public IActionResult GetAccountBalance([FromQuery] int userId)
  {
    var user = Users.FirstOrDefault(u => u.UserId == userId);
    if (user == null)
      return NotFound("User not found.");

    return Ok(new { user.UserId, user.Name, user.AccountBalance });
  }

  [HttpGet("user-details")]
  public IActionResult GetUserDetails([FromQuery] int userId)
  {
    var user = Users.FirstOrDefault(u => u.UserId == userId);
    if (user == null)
      return NotFound("User not found.");

    return Ok(user);
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
}
