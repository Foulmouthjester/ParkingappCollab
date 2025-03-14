using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Dtos
{
    public record RateScheduleDto(
        [Required][Range (0,23)]int StartHour,
        [Required][Range (0,23)]int EndHour,
        [Required][Range (0, 100)]double RatePerHour
        );
}
