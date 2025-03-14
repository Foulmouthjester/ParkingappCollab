using Microsoft.EntityFrameworkCore;
using ParkAppApi.Models;

namespace ParkingAppApi.Data;

public class ParkingDbContext : DbContext
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) 
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<ParkingSession> ParkingSessions => Set<ParkingSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Car>()
            .HasOne(c => c.User)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<ParkingSession>()
            .HasOne(s => s.Car)
            .WithMany(c => c.ParkingSessions)
            .HasForeignKey(s => s.CarId);
    }
}