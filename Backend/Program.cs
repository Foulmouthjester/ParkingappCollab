using Microsoft.AspNetCore.Builder; // For WebApplication
using Microsoft.EntityFrameworkCore; // For AddDbContext
using Microsoft.Extensions.DependencyInjection; // For AddCors, AddControllers, AddDbContext, AddEndpointsApiExplorer
using ParkingSystem.Data;
using Swashbuckle.AspNetCore.SwaggerGen; // For AddSwaggerGen
using Swashbuckle.AspNetCore.SwaggerUI; // For UseSwaggerUI
using Swashbuckle.AspNetCore.Swagger; // For UseSwagger

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


builder.Services.AddControllers();
// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=parkingapp.db")); // Use your DB connection string

// Add services to the container.

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
