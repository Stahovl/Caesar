using Caesar.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caesar.Infrastructure.Data;

public class CaesarDbContext : DbContext
{
    public CaesarDbContext(DbContextOptions<CaesarDbContext> options) : base(options) { }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>()
           .Property(m => m.Price)
           .HasColumnType("decimal(18,2)");
        // Здесь можно настроить связи между таблицами, если необходимо
    }
}
