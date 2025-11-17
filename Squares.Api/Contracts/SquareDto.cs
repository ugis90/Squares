namespace Squares.Api.Contracts;

public sealed record SquareDto(IReadOnlyCollection<PointWithIdDto> Points, double SideLength);

