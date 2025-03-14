using ParkingApp.Data;
using System.Text.Json.Serialization;

namespace ParkingApp.Models
{
    public class User
    {
       
        public int Id { get; private set; }
        public string Firstname { get; private set; } = string.Empty;
        public string Lastname { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;

        [JsonIgnore]
        public List<Car> Cars { get; private set; }
        [JsonIgnore]
        public Account Account { get; private set; }


        public User()
        {
            Cars = [];
            Account = new Account();
        }

        public User(string firstname, string lastname, string email, string password, List<Car>? cars = null)
        {
            
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
            Cars = cars ?? [];
            Account = new Account();
        }

        public void UpdateUser(string firstname, string lastname, string email, string password)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
        }

        public void AddCar(Car car)
        {
            Cars.Add(car);
        }

        public void RemoveCar(Car car)
        {
            if (!Cars.Remove(car))
                throw new KeyNotFoundException("Car not found.");
        }
    }
}
