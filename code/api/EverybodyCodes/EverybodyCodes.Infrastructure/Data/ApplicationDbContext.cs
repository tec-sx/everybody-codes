using EverybodyCodes.Infrastructure.Data.Configuration;
using EverybodyCodes.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EverybodyCodes.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<CameraEntity> Cameras { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Handle auto properties for all entities inheriting from EntityBase
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<int>("Id")
                    .HasColumnType("integer")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("date('now')")
                    .IsRequired();
            }
        }

        // Configure entities
        modelBuilder.ApplyConfiguration(new CameraEntityConfiguration());
    }
}
