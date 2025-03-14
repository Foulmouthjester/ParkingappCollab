using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record CarDto(
        [Required][Length(4, 10)]string Numberplate
        );
}
