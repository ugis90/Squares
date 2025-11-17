using Squares.Api.Contracts;
using Squares.Domain.Entities;
using Squares.Domain.Models;

namespace Squares.Api.Mappings;

public static class PointListMappings
{
    public static PointListResponse ToResponse(this PointList pointList)
    {
        var points = pointList.Points
            .OrderBy(point => point.X)
            .ThenBy(point => point.Y)
            .ThenBy(point => point.Id)
            .Select(point => point.ToDto())
            .ToArray();

        return new PointListResponse(pointList.Id, pointList.Name, points);
    }

    public static PointWithIdDto ToDto(this Point point) => new(point.Id, point.X, point.Y);

    public static SquareDto ToDto(this Square square)
    {
        var points = square.Points
            .Select(point => point.ToDto())
            .ToArray();

        return new SquareDto(points, Math.Round(square.SideLength, 6));
    }

    public static PointList ToDomain(this ImportPointListRequest request)
    {
        var points = request.Points?.Select(dto => new Point(dto.X, dto.Y));
        return new PointList(request.Name, points);
    }
}

