using Dealship.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Таблиці для збереження даних
    public DbSet<Car> Cars { get; set; }
    public DbSet<Engine> Engines { get; set; }
    public DbSet<Transmission> Transmissions { get; set; }
    public DbSet<FuelType> FuelTypes { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    // ➕ Додаємо таблицю користувачів (логін/реєстрація)
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Engine)
            .WithMany()
            .HasForeignKey(c => c.EngineId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Transmission)
            .WithMany()
            .HasForeignKey(c => c.TransmissionId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.FuelType)
            .WithMany()
            .HasForeignKey(c => c.FuelTypeId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Color)
            .WithMany()
            .HasForeignKey(c => c.ColorId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Car)
            .WithMany()
            .HasForeignKey(o => o.CarId);

        // Унікальний Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
