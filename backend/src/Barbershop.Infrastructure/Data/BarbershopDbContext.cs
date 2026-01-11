using Microsoft.EntityFrameworkCore;
using Barbershop.Domain.Entities;

namespace Barbershop.Infrastructure.Data;

public class BarbershopDbContext : DbContext
{
    public BarbershopDbContext(DbContextOptions<BarbershopDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ClientPhone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.ServiceType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.BarberName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.Time).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
