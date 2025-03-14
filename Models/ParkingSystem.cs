
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models;

public class User
{
  [Key]
  public string Email { get; set; } = string.Empty; // ✅ Prevents CS8618

  public string PasswordHash { get; set; } = string.Empty; // ✅ Prevents CS8618

  public decimal AccountBalance { get; set; } = 0;

  public List<string> Cars { get; set; } = new(); // ✅ No change needed

  [NotMapped] // Prevents EF from mapping this directly
  public string CarsSerialized
  {
    get => string.Join(",", Cars);
    set => Cars = string.IsNullOrEmpty(value) ? new List<string>() : value.Split(',').ToList();
  }
}


