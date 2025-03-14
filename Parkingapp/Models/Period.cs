using ParkingApp.Data;
using ParkingApp.Endpoints;
using System.Text.Json.Serialization;

namespace ParkingApp.Models
{
    public class Period
    {
       
        public int Id { get; private set; }
        public int CarId { get; set; }
        [JsonIgnore]
        public Car Car { get; set; } = null!;
        public double RateDaytime { get; set; } = 14;
        public double RateRestOfTime { get; set; } = 6;
        public DateTime StartTime {  get; set; }
        public DateTime? EndTime { get; set; }


        public Period() { }
        public Period(int carId, double rateDaytime, double rateRestOfTime, DateTime startTime)
        {
            CarId = carId;
            RateDaytime = rateDaytime;
            RateRestOfTime = rateRestOfTime;
            StartTime = startTime;
        }

        public Period(Car car)
        {
            Car = car;
            CarId = car.Id;
        }

        public void EndPeriod()
        {
            EndTime = DateTime.Now;
        }

        public void StartPeriod()
        {
            StartTime = DateTime.Now;
        }

        public string GetCurrentPeriod()
        {
            EndTime = DateTime.Now;
            return StartTime.ToString() + " - " + EndTime.ToString();
        }
        
        public int CalculateCost()
        {
            if (EndTime == null)
                EndTime = DateTime.Now;

            if (StartTime > EndTime) 
                throw new ArgumentException("Start time needs to be before end time.");

            double totalCost = 0;
            var current = StartTime;
           
            while (current < EndTime)
            {
                double hourlyRate = (current.Hour >= 8 && current.Hour < 18) ? 
                                     RateDaytime : RateRestOfTime;

                totalCost += hourlyRate/60;
                current = current.AddMinutes(1);
            }
            return (int)Math.Round(totalCost);
        }
    }
}
