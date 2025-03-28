
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }    
    public List<Car> Cars { get; set; } = new List<Car>();
  }

  public class Car
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();
  }

  public class ParkingSession
  {
    public int Id { get; set; }
    public int CarId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? Cost { get; set; }
    public Car Car { get; set; }
  }
}


