using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkAppApi.Models;
using ParkingAppApi.Data;
using ParkingAppApi.Models.Dtos;


namespace ParkAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingController : ControllerBase
{
    private readonly ParkingDbContext _context;

    public ParkingController(ParkingDbContext context)
    {
        _context = context;
    }

    // POST: api/parking/start
    [HttpPost("start")]
    public async Task<IActionResult> StartParking([FromBody] ParkingRequest request)
    {
        var car = await _context.Cars
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == request.CarId && c.UserId == request.UserId);

        if (car == null) return NotFound("Car not found");

        var session = new ParkingSession
        {
            CarId = car.Id,
            StartTime = DateTime.UtcNow
        };

        _context.ParkingSessions.Add(session);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Parking started", SessionId = session.Id });
    }

    // POST: api/parking/end
    [HttpPost("end")]
    public async Task<IActionResult> EndParking([FromBody] EndParkingRequest request)
    {
        var session = await _context.ParkingSessions
            .Include(s => s.Car)
            .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.Car.UserId == request.UserId);

        if (session == null) return NotFound("Session not found");

        session.EndTime = DateTime.UtcNow;
        session.TotalCost = CalculateParkingCost(session.StartTime, session.EndTime.Value);

        await _context.SaveChangesAsync();

        return Ok(new ParkingSessionDto
        {
            Id = session.Id,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            TotalCost = session.TotalCost,
            Car = new CarDto
            {
                Id = session.Car.Id,
                LicensePlate = session.Car.LicensePlate
            }
        });
    }

    // GET: api/parking/pricing
    [HttpGet("pricing")]
    public IActionResult GetPricing()
    {
        return Ok(new
        {
            DayRate = 14.0m,
            NightRate = 6.0m,
            DayHours = "08:00-18:00",
            Currency = "SEK"
        });
    }

    private static decimal CalculateParkingCost(DateTime start, DateTime end)
    {
        decimal total = 0;
        var current = start;
        
        while (current < end)
        {
            var rate = current.Hour is >= 8 and < 18 ? 14.0m : 6.0m;
            var minutes = (decimal)Math.Min((end - current).TotalMinutes, 60);
            total += rate * (minutes / 60);
            current = current.AddHours(1);
        }
        
        return Math.Round(total, 2);
    }
}

public record ParkingRequest(int UserId, int CarId);
public record EndParkingRequest(int UserId, int SessionId);