using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingApp.Data;
using ParkingApp.Models;

namespace ParkingApp.Endpoints
{
    public static class CarEndpoints
    {
        const string getCarEndpoint = "Get Car";

        public static RouteGroupBuilder MapCarEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("users/{userId}/cars").WithParameterValidation();


            group.MapGet("/{numberPlate}", async (ParkingContext context, int userId, string numberPlate) =>
            {
                var user = await context.Users
                        .Include(u => u.Cars)
                        .SingleOrDefaultAsync(u => u.Id == userId);

                if (user is null)
                    return Results.NotFound($"User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Numberplate == numberPlate);
                return car is null ? Results.NotFound("Car not found.") : Results.Ok(car);
            }).WithName(getCarEndpoint);

            group.MapPost("/addcar", async (ParkingContext context, int userId, [FromBody] string numberPlate) =>
            {
                var carService = new CarService(context);
                var result = await carService.AddCar(userId, numberPlate);

                if (!result)
                {
                    return Results.NotFound("User not found or car with the same number plate already exists.");
                }

                var car = await context.Cars.SingleOrDefaultAsync(c => c.Numberplate == numberPlate && c.UserId == userId);
                return Results.CreatedAtRoute(getCarEndpoint, new { userId, numberPlate }, car);
            });

            group.MapGet("/", async (ParkingContext context, int userId) =>
            {
                var user = await context.Users
                        .Include(u => u.Cars)
                        .ThenInclude(c => c.Period)
                        .SingleOrDefaultAsync(u => u.Id == userId);

                return user is null ? Results.NotFound() : Results.Ok(user.Cars);
            });



            return group;   
        }


    }
}
