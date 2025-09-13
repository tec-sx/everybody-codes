using EverybodyCodes.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EverybodyCodes.Infrastructure.Data.Configuration;

internal sealed class CameraEntityConfiguration : IEntityTypeConfiguration<CameraEntity>
{
    public void Configure(EntityTypeBuilder<CameraEntity> builder)
    {
        builder.ToTable("Camera");

        builder
            .Property(b => b.Number)
            .HasColumnType("integer")
            .IsRequired();

        builder
            .Property(b => b.Name)
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder
            .Property(b => b.Latitude)
            .HasColumnType("float")
            .IsRequired();

        builder
            .Property(b => b.Longitude)
            .HasColumnType("float")
            .IsRequired();
    }
}
