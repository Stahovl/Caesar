using Caesar.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caesar.Infrastructure.Data;

public class CaesarDbContext : DbContext
{
    public CaesarDbContext(DbContextOptions<CaesarDbContext> options) : base(options) { }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MenuItem>()
           .Property(m => m.Price)
           .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);
        // Seed admin user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // В реальном приложении используйте более сложный пароль
                Role = "Admin"
            }
        );
    }
}