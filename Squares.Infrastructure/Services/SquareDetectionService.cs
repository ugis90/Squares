using Squares.Domain.Abstractions;
using Squares.Domain.Entities;
using Squares.Domain.Models;

namespace Squares.Infrastructure.Services;

public class SquareDetectionService : ISquareDetectionService
{
    public IReadOnlyList<Square> GetSquares(IReadOnlyCollection<Point> points)
    {
        var results = new List<Square>();
        EnumerateSquares(points, results);
        return results;
    }

    public int CountSquares(IReadOnlyCollection<Point> points)
    {
        return EnumerateSquares(points, squares: null);
    }

    private static int EnumerateSquares(IReadOnlyCollection<Point> points, List<Square>? squares)
    {
        if (points.Count < 4)
        {
            return 0;
        }

        var pointArray = points.ToArray();
        var diagonals = BuildDiagonals(pointArray);
        var count = 0;

        foreach (var pairList in diagonals.Values.Where(pairList => pairList.Count >= 2))
        {
            for (var i = 0; i < pairList.Count - 1; i++)
            {
                for (var j = i + 1; j < pairList.Count; j++)
                {
                    var pairA = pairList[i];
                    var pairB = pairList[j];

                    if (pairA.SharesEndpointWith(pairB))
                    {
                        continue;
                    }

                    if (!pairA.IsPerpendicularTo(pairB))
                    {
                        continue;
                    }

                    count++;

                    if (squares is null) continue;
                    var squarePoints = new[] { pairA.First, pairA.Second, pairB.First, pairB.Second };
                    var sideLength = Math.Sqrt(pairA.DistanceSquared / 2.0);
                    squares.Add(new Square(squarePoints, sideLength));
                }
            }
        }

        return count;
    }

    private static Dictionary<DiagonalKey, List<PointPair>> BuildDiagonals(IReadOnlyList<Point> points)
    {
        var result = new Dictionary<DiagonalKey, List<PointPair>>();

        for (var i = 0; i < points.Count - 1; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                var p1 = points[i];
                var p2 = points[j];

                var dx = (long)p1.X - p2.X;
                var dy = (long)p1.Y - p2.Y;
                var distanceSquared = dx * dx + dy * dy;

                if (distanceSquared == 0)
                {
                    continue;
                }

                var sumX = (long)p1.X + p2.X;
                var sumY = (long)p1.Y + p2.Y;
                var key = new DiagonalKey(sumX, sumY, distanceSquared);
                if (!result.TryGetValue(key, out var list))
                {
                    list = new List<PointPair>();
                    result[key] = list;
                }

                list.Add(new PointPair(p1, p2, distanceSquared));
            }
        }

        return result;
    }

    private readonly record struct DiagonalKey(long SumX, long SumY, long DistanceSquared);

    private readonly record struct PointPair(Point First, Point Second, long DistanceSquared)
    {
        public bool SharesEndpointWith(PointPair other)
        {
            return First == other.First || First == other.Second || Second == other.First || Second == other.Second;
        }

        public bool IsPerpendicularTo(PointPair other)
        {
            var dx1 = (long)First.X - Second.X;
            var dy1 = (long)First.Y - Second.Y;
            var dx2 = (long)other.First.X - other.Second.X;
            var dy2 = (long)other.First.Y - other.Second.Y;

            var dotProduct = dx1 * dx2 + dy1 * dy2;
            return dotProduct == 0;
        }
    }
}

