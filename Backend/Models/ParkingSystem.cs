
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models;

public class User
{
  [Key]
  public string Email { get; set; } = string.Empty; 

  public string PasswordHash { get; set; } = string.Empty; 

  public decimal AccountBalance { get; set; } = 0;

  public List<string> Cars { get; set; } = new(); 

  [NotMapped] 
  public string CarsSerialized
  {
    get => string.Join(",", Cars);
    set => Cars = string.IsNullOrEmpty(value) ? new List<string>() : value.Split(',').ToList();
  }
}


