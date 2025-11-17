namespace Squares.Domain.Entities;

public class Point
{
    private Point()
    {
        // Required by EF Core
    }

    public Point(int x, int y)
        : this(Guid.NewGuid(), x, y)
    {
    }

    public Point(Guid id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public Guid Id { get; private set; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public Guid PointListId { get; private set; }

    public void AssignToList(Guid pointListId)
    {
        if (pointListId == Guid.Empty)
        {
            throw new ArgumentException("Point list id must be provided.", nameof(pointListId));
        }

        PointListId = pointListId;
    }

    public override string ToString() => $"({X}, {Y})";
}

