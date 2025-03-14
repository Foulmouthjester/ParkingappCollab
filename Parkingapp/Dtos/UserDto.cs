using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record UserDto(
        int Id,
        [Required][Length(2, 30)] string Firstname,
        [Required][Length(2, 30)] string Lastname,
        [Required][Length(5, 50)] string Email,
        [Required][Length(7, 30)] string Password
        );
}
