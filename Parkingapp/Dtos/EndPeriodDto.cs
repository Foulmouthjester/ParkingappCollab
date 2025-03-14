using ParkingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Dtos
{
    public record EndPeriodDto(
        [Required]Car car
    );
}
