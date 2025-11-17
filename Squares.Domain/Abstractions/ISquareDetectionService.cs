using Squares.Domain.Entities;
using Squares.Domain.Models;

namespace Squares.Domain.Abstractions;

public interface ISquareDetectionService
{
    IReadOnlyList<Square> GetSquares(IReadOnlyCollection<Point> points);

    int CountSquares(IReadOnlyCollection<Point> points);
}

