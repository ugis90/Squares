namespace Squares.Domain.Entities;

public class PointList
{
    private readonly List<Point> _points = new();

    private PointList()
    {
        // Required by EF Core
    }

    public PointList(string name, IEnumerable<Point>? points = null)
        : this(Guid.NewGuid(), name, points)
    {
    }

    public PointList(Guid id, string name, IEnumerable<Point>? points = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Point list name is required.", nameof(name));
        }

        Id = id;
        Name = name.Trim();

        if (points is null)
        {
            return;
        }

        foreach (var point in points)
        {
            AddPoint(point);
        }
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<Point> Points => _points;

    public Point AddPoint(int x, int y)
    {
        var point = new Point(x, y);
        AddPoint(point);
        return point;
    }

    public void AddPoint(Point point)
    {
        if (point.PointListId == Guid.Empty)
        {
            point.AssignToList(Id);
        }
        else if (point.PointListId != Id)
        {
            throw new InvalidOperationException("Point is already assigned to a different list.");
        }

        _points.Add(point);
    }

    public bool RemovePoint(Guid pointId)
    {
        var index = _points.FindIndex(p => p.Id == pointId);
        if (index == -1)
        {
            return false;
        }

        _points.RemoveAt(index);
        return true;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Point list name is required.", nameof(name));
        }

        Name = name.Trim();
    }
}

