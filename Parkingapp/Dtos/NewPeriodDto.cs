using ParkingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record NewPeriodDto(
        [Required]int CarId,
        double RateDayTime,
        double RateRestOfTime,
        DateTime StartTime
        );
}
