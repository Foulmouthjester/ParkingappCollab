using Microsoft.EntityFrameworkCore;
using ParkingSystem.Models;

namespace ParkingSystem.Data;

public class ParkingDbContext : DbContext
{
  public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options) { }

  public DbSet<User> Users { get; set; }
  public DbSet<ParkingPeriod> ParkingPeriods { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
        .HasKey(u => u.Email); // Use Email as Primary Key

    modelBuilder.Entity<ParkingPeriod>()
        .HasKey(p => p.Id); // Primary Key for ParkingPeriod
  }
}

