using Squares.Domain.Entities;
using Squares.Infrastructure.Services;

namespace Squares.Tests.Services;

public class SquareDetectionServiceTests
{
    private readonly SquareDetectionService _service = new();

    [Fact]
    public void CountSquares_ReturnsZero_ForLessThanFourPoints()
    {
        var points = new[]
        {
            CreatePoint(0, 0),
            CreatePoint(1, 0),
            CreatePoint(1, 1)
        };

        var result = _service.CountSquares(points);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Detects_Single_AxisAligned_Square()
    {
        var points = new[]
        {
            CreatePoint(-1, -1),
            CreatePoint(1, -1),
            CreatePoint(1, 1),
            CreatePoint(-1, 1)
        };

        var result = _service.GetSquares(points);

        Assert.Single(result);
        Assert.Equal(4, result[0].Points.Count);
        Assert.Equal(2d, result[0].SideLength, precision: 5);
    }

    [Fact]
    public void Detects_Rotated_Square()
    {
        var points = new[]
        {
            CreatePoint(0, 2),
            CreatePoint(2, 0),
            CreatePoint(0, -2),
            CreatePoint(-2, 0)
        };

        var count = _service.CountSquares(points);

        Assert.Equal(1, count);
    }

    [Fact]
    public void Detects_Multiple_Squares_In_Grid()
    {
        var points = new List<Point>();

        for (var x = 0; x <= 2; x++)
        {
            for (var y = 0; y <= 2; y++)
            {
                points.Add(CreatePoint(x, y));
            }
        }

        var count = _service.CountSquares(points);

        Assert.Equal(6, count);
    }

    [Fact]
    public void Ignores_Rectangles_With_Equal_Diagonals()
    {
        var points = new[]
        {
            CreatePoint(0, 0),
            CreatePoint(4, 0),
            CreatePoint(4, 2),
            CreatePoint(0, 2)
        };

        var count = _service.CountSquares(points);

        Assert.Equal(0, count);
    }

    private static Point CreatePoint(int x, int y) => new(x, y);
}

