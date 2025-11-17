using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Squares.Domain.Entities;

namespace Squares.Infrastructure.Persistence.Configurations;

public class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.ToTable("Points");

        builder.HasKey(point => point.Id);

        builder.Property(point => point.X).IsRequired();
        builder.Property(point => point.Y).IsRequired();
        builder.Property(point => point.PointListId).IsRequired();

        builder.HasIndex(point => new { point.PointListId, point.X, point.Y });
    }
}

