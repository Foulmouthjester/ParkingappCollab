using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkAppApi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    public List<Car> Cars { get; set; } = new();
}

public class Car
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string LicensePlate { get; set; } = string.Empty;
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public List<ParkingSession> ParkingSessions { get; set; } = new();
}

public class ParkingSession
{
    [Key]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? TotalCost { get; set; }
    
    [ForeignKey("Car")]
    public int CarId { get; set; }
    public Car? Car { get; set; }
}