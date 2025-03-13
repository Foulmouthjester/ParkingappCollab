using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models
{
  public class ParkingPeriod
  {
    [Key]
    public int Id { get; set; } // Primary Key (Auto-increment)

    [Required]
    public string LicensePlate { get; set; }

    [Required]
    public DateTime StartTime { get; set; }
  }
}
