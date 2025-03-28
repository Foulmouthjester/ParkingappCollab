using ParkingSystem.Models;

public class ParkingSession
{
  public int Id { get; set; }
  public int CarId { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime? EndTime { get; set; }
  public decimal? Cost { get; set; }
  public Car Car { get; set; } // Line 8: Car isn’t in Backend.Models
}

