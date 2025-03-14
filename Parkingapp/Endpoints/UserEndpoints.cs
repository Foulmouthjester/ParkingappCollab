using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingApp.Data;
using ParkingApp.Models;

namespace ParkingApp.Endpoints
{
    public static class UserEndpoints
    {
        const string getUserEndpoint = "Get User";
        const string getCarEndpoint = "Get Car";
        const string getPeriodEndpoint = "Get Period";

        public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("users").WithParameterValidation();

            group.MapGet("/", (ParkingContext context) =>
            {
                return Results.Ok(context.Users);
            });

            group.MapGet("/{userId}/cars", async (ParkingContext context, int userId) =>
            {
                var user = await context.Users
                        .Include(u => u.Cars)
                        .ThenInclude(c => c.Period)
                        .SingleOrDefaultAsync(u => u.Id == userId);

                return user is null ? Results.NotFound() : Results.Ok(user.Cars);
            });

            group.MapGet("/{id}", async (ParkingContext context, int id) =>
            {
                var user = await context.Users.FindAsync(id);

                return user is null ? Results.NotFound($"User with id:{id} was not found.") : 
                                      Results.Ok(user);

            }).WithName(getUserEndpoint);

            group.MapGet("/{userId}/account/debt", async (int userId, ParkingContext context) =>
            {
                var user = await context.Users
                .Include(u => u.Account)
                .SingleOrDefaultAsync(x => x.Id == userId);

                if (user is null)
                    return Results.NotFound("User not found.");

                user.Account.CalculateDebt();
                return Results.Ok($"{user.Account.CalculateDebt():C}");
            });

            group.MapGet("/{userId}/cars/{carId}/period/activeperiod", async (ParkingContext context, int userId, int carId) =>
            {
                var user = await context.Users
                          .Include(u => u.Cars)
                          .ThenInclude(c => c.Periods)
                          .SingleOrDefaultAsync(x => x.Id == userId);

                if (user is null)
                    return Results.NotFound("User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Id == carId);

                if (car is null)
                    return Results.NotFound("Car not found.");

                if (car.Period is null)
                    return Results.NotFound("No active period found for this car.");

                return Results.Ok($"{car.Period.GetCurrentPeriod()}     {car.Period.CalculateCost():C}");
            }).WithName(getPeriodEndpoint);


            group.MapPost("/{userId}/cars/{carId}/period/start", async (ParkingContext context, int userId, int carId, [FromBody] NewPeriodDto newPeriod) =>
            {
                var user = await context.Users
                           .Include(u => u.Cars)
                           .SingleOrDefaultAsync(x => x.Id == userId);

                if (user is null)
                    return Results.NotFound("User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Id == newPeriod.CarId);

                if (car is null)
                    return Results.NotFound("Car not found.");

                var existingPeriod = await context.Periods.SingleOrDefaultAsync(p => p.CarId == newPeriod.CarId);
                if (existingPeriod != null)
                    return Results.Conflict("A period with the same CarId already exists.");

                var period = new Period
                {
                    CarId = newPeriod.CarId,
                    Car = car,
                    RateDaytime = newPeriod.RateDayTime,
                    RateRestOfTime = newPeriod.RateRestOfTime,
                    StartTime = newPeriod.StartTime
                };

                car.StartPeriod(period);

                context.Periods.Add(period);
                await context.SaveChangesAsync();

                return Results.CreatedAtRoute(getPeriodEndpoint, new { id = period.Id }, period);
            });


            group.MapPut("/{userId}/cars/{numberPlate}/period/end", async (ParkingContext context, int userId, string numberPlate) =>
            {
                var user = await context.Users
                           .Include(u => u.Cars)
                           .ThenInclude(c => c.Period)
                           .SingleOrDefaultAsync(x => x.Id == userId);

                if (user is null)
                    return Results.NotFound("User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Numberplate == numberPlate);

                if (car is null)
                    return Results.NotFound("Car not found.");

                var period = car.Period;

                if (period is null)
                    return Results.NotFound("No active period found for this car.");

                period.EndPeriod();
                period.CalculateCost();
                context.Remove(period);

                await context.SaveChangesAsync();

                return Results.Ok($"{period.GetCurrentPeriod()}     {period.CalculateCost():C}");
            });


            group.MapPost("/{userId}/cars/addcar", async (ParkingContext context, int userId, [FromBody] string numberPlate) =>
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


            group.MapGet("/{userId}/cars/{numberPlate}", async (ParkingContext context, int userId, string numberPlate) =>
            {
                var user = await context.Users
                        .Include(u => u.Cars)
                        .SingleOrDefaultAsync(u => u.Id == userId);

                if (user is null)
                    return Results.NotFound($"User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Numberplate == numberPlate);
                return car is null ? Results.NotFound("Car not found.") : Results.Ok(car);
            }).WithName(getCarEndpoint);


            group.MapPost("/", async (ParkingContext context, NewUserDto user) =>
            {
                var newUser = new User(
                    user.Firstname,
                    user.Lastname,
                    user.Email,
                    user.Password
                );

                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                return Results.CreatedAtRoute(getUserEndpoint, new { id = newUser.Id }, newUser);
            });

            return group;
        }
    }
}
