namespace Squares.Api.Contracts;

public sealed record PointListResponse(Guid Id, string Name, IReadOnlyCollection<PointWithIdDto> Points);

