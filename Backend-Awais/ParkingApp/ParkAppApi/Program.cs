using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ParkingAppApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Add services
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
    db.Database.Migrate();
}

app.Run();

// Helper method to calculate parking cost
/*decimal CalculateParkingCost(DateTime startTime, DateTime endTime)
{
    decimal totalCost = 0;
    var currentTime = startTime;

    while (currentTime < endTime)
    {
        var nextHour = currentTime.AddHours(1);
        if (nextHour > endTime)
        {
            nextHour = endTime;
        }

        // Check if the current hour is between 8 AM and 6 PM
        if (currentTime.Hour >= 8 && currentTime.Hour < 18)
        {
            totalCost += 14 * (decimal)(nextHour - currentTime).TotalHours;
        }
        else
        {
            totalCost += 6 * (decimal)(nextHour - currentTime).TotalHours;
        }

        currentTime = nextHour;
    }

    return totalCost;
}*/


