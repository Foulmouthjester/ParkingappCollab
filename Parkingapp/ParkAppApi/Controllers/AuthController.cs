using Microsoft.EntityFrameworkCore;
using ParkingAppApi.Data;

namespace ParkAppApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ParkingDbContext _context;

    public AuthController(ParkingDbContext context)
    {
        _context = context;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistration request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Registration successful" });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        return Ok(new { Message = "Login successful", UserId = user.Id });
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var newHash = HashPassword(password);
        return newHash == storedHash;
    }
}

public record UserRegistration(string Email, string Password);
public record UserLogin(string Email, string Password);