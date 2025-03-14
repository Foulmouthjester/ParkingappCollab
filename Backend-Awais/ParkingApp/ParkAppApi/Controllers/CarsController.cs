using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkAppApi.Models;
using ParkingAppApi.Data;

namespace ParkAppApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ParkingDbContext _context;

    public CarsController(ParkingDbContext context)
    {
        _context = context;
    }

    // POST: api/cars
    [HttpPost]
    public async Task<IActionResult> AddCar([FromBody] CarRegistration request)
    {
        var user = await _context.Users
            .Include(u => u.Cars)
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null) return NotFound("User not found");

        var car = new Car
        {
            LicensePlate = request.LicensePlate,
            UserId = user.Id
        };

        user.Cars.Add(car);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Car registered successfully", CarId = car.Id });
    }
}

public record CarRegistration(int UserId, string LicensePlate);