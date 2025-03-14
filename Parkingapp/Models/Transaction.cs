using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ParkingApp.Models
{
    public class Transaction
    {
        public int Id { get; private set; }
        public double Amount { get; private set; }
        public DateTime Date { get; private set; }
        public int AccountId { get; private set; }
        public Account Account { get; private set; } = null!;

        public Transaction() { }
        public Transaction(double amount, DateTime date, Account account)
        {
            Amount = amount;
            Date = date;
            Account = account ?? throw new ArgumentNullException(nameof(account));
            AccountId = account.Id;
        }
    }
}
