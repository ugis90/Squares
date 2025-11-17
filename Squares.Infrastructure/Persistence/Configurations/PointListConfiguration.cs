using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Squares.Domain.Entities;

namespace Squares.Infrastructure.Persistence.Configurations;

public class PointListConfiguration : IEntityTypeConfiguration<PointList>
{
    private const int NameMaxLength = 200;

    public void Configure(EntityTypeBuilder<PointList> builder)
    {
        builder.ToTable("PointLists");

        builder.HasKey(pl => pl.Id);

        builder.Property(pl => pl.Name)
            .HasMaxLength(NameMaxLength)
            .IsRequired();

        builder.Metadata
            .FindNavigation(nameof(PointList.Points))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(pl => pl.Points)
            .WithOne()
            .HasForeignKey(point => point.PointListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

