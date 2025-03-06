
using System;


namespace ParkingSystem.Models;

public class User
{
  public int UserId { get; set; }
  public string Name { get; set; }
  public List<string> Cars { get; set; } = new();
  public decimal AccountBalance { get; set; }
}

public class ParkingPeriod
{
  public string LicensePlate { get; set; }
  public DateTime StartTime { get; set; }
}
