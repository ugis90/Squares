using Squares.Domain.Entities;

namespace Squares.Domain.Abstractions;

public interface IPointListRepository
{
    Task<PointList?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PointList>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(PointList pointList, CancellationToken cancellationToken = default);

    Task<Point?> AddPointAsync(Guid pointListId, Point point, CancellationToken cancellationToken = default);

    Task<bool> DeletePointAsync(Guid pointListId, Guid pointId, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

