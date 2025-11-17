using Squares.Domain.Entities;

namespace Squares.Domain.Models;

public sealed class Square
{
    private const int RequiredPointCount = 4;

    public Square(IEnumerable<Point> points, double sideLength)
    {
        var ordered = points
            .OrderBy(p => p.X)
            .ThenBy(p => p.Y)
            .ThenBy(p => p.Id)
            .ToArray();

        if (ordered.Length != RequiredPointCount)
        {
            throw new ArgumentException($"A square must contain {RequiredPointCount} points.", nameof(points));
        }

        if (sideLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sideLength), "Side length must be positive.");
        }

        Points = ordered;
        SideLength = sideLength;
    }

    public IReadOnlyList<Point> Points { get; }

    public double SideLength { get; }
}

