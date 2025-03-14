using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record EditCarDto(
        [Required][Length(4, 10)] string Numberplate
        );
}
