using Caesar.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caesar.Infrastructure.Data;


/// <summary>
/// Represents the Entity Framework database context for the application.
/// Сontext is responsible for creating the database tables, setting field validations,
/// and seeding the initial admin user after the tables are created.
/// </summary>
public class CaesarDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CaesarDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public CaesarDbContext(DbContextOptions<CaesarDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Reservations table.
    /// </summary>
    public DbSet<Reservation> Reservations { get; set; }

    /// <summary>
    /// Gets or sets the MenuItems table.
    /// </summary>
    public DbSet<MenuItem> MenuItems { get; set; }

    /// <summary>
    /// Gets or sets the Order table.
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets or sets the OrderItems table.
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Gets or sets the Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Configures the schema needed for the context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>

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

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany()
            .HasForeignKey(oi => oi.MenuItemId);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            }
        );
    }
}