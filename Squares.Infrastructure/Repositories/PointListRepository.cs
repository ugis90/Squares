using Microsoft.EntityFrameworkCore;
using Squares.Domain.Abstractions;
using Squares.Domain.Entities;
using Squares.Infrastructure.Persistence;

namespace Squares.Infrastructure.Repositories;

public class PointListRepository(SquaresDbContext dbContext) : IPointListRepository
{
    public async Task<PointList?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.PointLists
            .Include(list => list.Points)
            .FirstOrDefaultAsync(list => list.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<PointList>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.PointLists
            .Include(list => list.Points)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PointList pointList, CancellationToken cancellationToken = default)
    {
        await dbContext.PointLists.AddAsync(pointList, cancellationToken);
    }

    public async Task<Point?> AddPointAsync(Guid pointListId, Point point, CancellationToken cancellationToken = default)
    {
        var list = await dbContext.PointLists
            .Include(l => l.Points)
            .FirstOrDefaultAsync(l => l.Id == pointListId, cancellationToken);

        if (list is null)
        {
            return null;
        }

        list.AddPoint(point);
        dbContext.Points.Add(point);
        return point;
    }

    public async Task<bool> DeletePointAsync(Guid pointListId, Guid pointId, CancellationToken cancellationToken = default)
    {
        var list = await dbContext.PointLists
            .Include(l => l.Points)
            .FirstOrDefaultAsync(l => l.Id == pointListId, cancellationToken);

        return list is not null && list.RemovePoint(pointId);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}

