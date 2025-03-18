using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using System.Security.Cryptography;
using System.Text;
using ParkingSystem.DTOs;
using BCrypt.Net;

namespace ParkingSystem.Controllers
{
  [ApiController]
  [Route("api/auth")]
  
  public class AuthController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
      _context = context;
    }

    // Register endpoint (POST /api/auth/register)
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
      var existingUser = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
      if (existingUser != null)
      {
        return BadRequest("User already exists");
      }

      var newUser = new User
      {
        Email = userDto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
        AccountBalance = 0
      };

      _context.Users.Add(newUser);
      _context.SaveChanges();

      return Ok(new { Message = "Registration successful!" });
    }

    // Login endpoint (POST /api/auth/login)
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
      var user = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
      if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
      {
        return Unauthorized("Invalid credentials");
      }

      // Generate JWT token or return success message
      return Ok(new { Message = "Login successful!" });
    }
  }
}

