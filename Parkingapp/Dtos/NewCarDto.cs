using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record NewCarDto(
        [Required][Length(4, 10)] string Numberplate
        );
}
