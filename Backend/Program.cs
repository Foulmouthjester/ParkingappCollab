using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

const string sitePolicy = "MyPolicy";

// CORS Policy
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: sitePolicy, built =>
  {
    built.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
  });
});



// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=parkingapp.db")); // Use your DB connection string

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//app.UseDeveloperExceptionPage();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(sitePolicy);
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
