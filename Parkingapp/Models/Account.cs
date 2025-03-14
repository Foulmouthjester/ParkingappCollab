using ParkingApp.Data;

namespace ParkingApp.Models
{
    public class Account
    {
        
        public int Id { get; private set; }
        public double Debt { get; private set; } = 0;
        public ICollection<Transaction> Transactions { get; set; } = [];


        public Account() 
            {
            }
       

        public void MakePayment(Transaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));

            if (transaction.Amount > Debt)
                throw new InvalidOperationException("Payment exceeds the current debt.");

            Transactions.Add(transaction);
            Debt -= transaction.Amount;
        }

        public void AddParkingDebt(Transaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));

            Transactions.Add(transaction);
            Debt += transaction.Amount;
        }


        public void ClearDebt()
        {
            Debt = 0;
        }

        public double CalculateDebt()
        {
            return Transactions.Sum(t => t.Amount);
        }
    }
}
