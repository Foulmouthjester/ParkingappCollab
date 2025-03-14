
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ParkingApp.Models
{
    public class Car
    {
        public int Id { get; private set; }
        [Required]
        [Length(4, 10)]
        public string Numberplate { get; private set; }
        public Period? Period { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public required User User { get; set; }
        public List<Period> Periods { get; set; } = [];


        public Car(string numberplate)
        {
            Numberplate = numberplate;
        }

        public void StartPeriod(Period period)
        {
            Period = period;
            Period.StartPeriod();
        }

        public void RemovePeriod()
        {
            Period = null;
        }


    }


}
