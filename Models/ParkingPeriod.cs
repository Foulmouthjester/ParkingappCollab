using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models
{
  public class ParkingPeriod
  {
    [Key]
    public int Id { get; set; } // Primary Key (Auto-increment)

    [Required]
    public string LicensePlate { get; set; } = string.Empty; // ✅ Prevents CS8618

    [Required]
    public DateTime StartTime { get; set; } // ✅ No need for default here

    // ✅ Default constructor required by EF Core
    public ParkingPeriod()
    {
      this.LicensePlate = string.Empty; // ✅ Correct property initialization
      this.StartTime = DateTime.Now; // ✅ Correct property initialization
    }

    // ✅ Parameterized constructor for manual object creation
    public ParkingPeriod(string licensePlate)
    {
      this.LicensePlate = licensePlate;
      this.StartTime = DateTime.Now;
    }
  }
}


