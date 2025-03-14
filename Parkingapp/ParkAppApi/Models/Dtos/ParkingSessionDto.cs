namespace ParkingAppApi.Models.Dtos;

public class ParkingSessionDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? TotalCost { get; set; }
    public CarDto Car { get; set; }
}

public class CarDto
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }
}