
using Microsoft.EntityFrameworkCore;
using ParkingApp.Data;
using ParkingApp.Endpoints;
using ParkingApp.Models;

public class CarService(ParkingContext context)
{
    private readonly ParkingContext _context = context;

    public async Task<List<Car>> GetCarsByUserId(int userId)
    {
        return await _context.Cars
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> AddCar(int userId, string numberPlate)
    {
        var user = await _context.Users.Include(u => u.Cars).SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        if (user.Cars.Any(c => c.Numberplate == numberPlate))
        {
            return false;
        }

        var car = new Car(numberPlate)
        {
            UserId = userId,
            User = user
        };

        user.Cars.Add(car);
        await _context.SaveChangesAsync();
        return true;
    }
}

