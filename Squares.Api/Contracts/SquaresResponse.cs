namespace Squares.Api.Contracts;

public sealed record SquaresResponse(int Count, IReadOnlyCollection<SquareDto> Squares);

