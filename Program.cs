using Backend.Services;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseSqlite("Data Source=parkingapp.db"));

builder.Services.AddScoped<ParkingCostService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowReactApp", policy =>
  {
    policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
  });
});

var app = builder.Build();

// Configure middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowReactApp"); // Must be before UseAuthorization and MapControllers
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

// Optional: Seed data (uncomment if needed)
/*
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Email = "user1@example.com", PasswordHash = "hash1", AccountBalance = 0m },
            new User { Email = "user2@example.com", PasswordHash = "hash2", AccountBalance = 0m }
        );
        context.SaveChanges();
        Console.WriteLine("Seeded users: user1 (Id=1), user2 (Id=2)");
    }
}
*/

app.Run();
