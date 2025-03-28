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
    public decimal AccountBalance { get; set; }
    public List<Car> Cars { get; set; } = new List<Car>(); // Ensure this is correct
  }

  public class Car
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
  }
}


