namespace Backend.Services
{
    public class ParkingCostService
    {
        private const decimal DayRate = 14.00m;   // SEK, 08:00-18:00
        private const decimal NightRate = 6.00m;  // SEK, 18:00-08:00
        private const int DayStartHour = 8;       // 08:00
        private const int DayEndHour = 18;        // 18:00 (exclusive)
        private const string Currency = "SEK";

        public decimal CalculateCost(DateTime startTime, DateTime? endTime)
        {
            var end = endTime ?? DateTime.UtcNow;
            if (end < startTime) throw new ArgumentException("End time must be after start time.");

            decimal totalCost = 0m;
            DateTime current = startTime;

            // Iterate hour-by-hour until we reach or exceed end time
            while (current < end)
            {
                // Determine if the current hour falls in day or night
                int currentHour = current.Hour;
                decimal rate = (currentHour >= DayStartHour && currentHour < DayEndHour) ? DayRate : NightRate;

                // Calculate minutes in this hour (up to 60)
                DateTime nextHour = current.AddHours(1);
                decimal minutesInHour = (decimal)Math.Min((end - current).TotalMinutes, 60);

                // Prorate the hourly rate based on minutes used
                decimal hourCost = rate * (minutesInHour / 60);
                totalCost += hourCost;

                // Move to the next hour
                current = nextHour;
            }

            // Log for debugging
            Console.WriteLine($"Total cost calculated: {totalCost} {Currency} from {startTime} to {end}");
            return totalCost;
        }
    }
}