
using ParkingApp.Models;
using Microsoft.EntityFrameworkCore;


namespace ParkingApp.Data
{
    public class ParkingContext(DbContextOptions<ParkingContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Period> Periods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Cars)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Periods)
                .WithOne(p => p.Car)
                .HasForeignKey(p => p.CarId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
