using Microsoft.EntityFrameworkCore;
using Squares.Domain.Entities;

namespace Squares.Infrastructure.Persistence;

public class SquaresDbContext(DbContextOptions<SquaresDbContext> options) : DbContext(options)
{
    public DbSet<PointList> PointLists => Set<PointList>();

    public DbSet<Point> Points => Set<Point>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SquaresDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

