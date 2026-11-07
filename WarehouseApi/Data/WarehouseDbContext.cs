using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;

namespace WarehouseApi.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200);
            e.Property(p => p.Category).HasMaxLength(100);
            e.Property(p => p.Variant).HasMaxLength(200);
            e.Property(p => p.Unit).HasMaxLength(20);
            e.Property(p => p.Price).HasPrecision(18, 2);
            e.Property(p => p.Quantity).HasPrecision(18, 2);
            e.HasIndex(p => new { p.Name, p.Variant }).IsUnique();
        });

        b.Entity<Sale>(e =>
        {
            e.Property(s => s.Quantity).HasPrecision(18, 2);
            e.Property(s => s.UnitPrice).HasPrecision(18, 2);
        });
    }
}