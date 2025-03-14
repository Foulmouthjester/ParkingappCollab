using ParkingApp.Data;
using ParkingApp.Dtos;
using ParkingApp.Models;

namespace ParkingApp.Endpoints
{
    public static class PeriodEndpoints
    {
        public static RouteGroupBuilder MapPeriodEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("periods").WithParameterValidation();





            return group;
        }

    }
}
