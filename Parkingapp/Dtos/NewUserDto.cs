using ParkingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record NewUserDto(
        [Required][Length(2, 30)] string Firstname,
        [Required][Length(2, 30)] string Lastname,
        [Required][EmailAddress][Length(5, 50)] string Email,
        [Required][Length(7, 30)] string Password
        //[Required] string Car
        );
}
