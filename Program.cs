using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add SQLite Database
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseSqlite("Data Source=parking.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
  db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
