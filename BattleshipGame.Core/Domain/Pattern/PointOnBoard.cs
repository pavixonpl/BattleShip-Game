namespace BattleshipGame.Core.Domain.Pattern;
public class PointOnBoard
{
    public PointOnBoard(int x, int y)
    {
        X = x;
        Y = y;
    }

    private PointOnBoard()
    {

    }
    public int X { get; }
    public int Y { get; }

    public static bool operator ==(PointOnBoard a, PointOnBoard b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Y == b.Y && a.X == b.X;
    }

    public static bool operator !=(PointOnBoard a, PointOnBoard b)
    {
        return !(a == b);
    }

    public bool IsAboveThePoint(PointOnBoard point) => point.X == X && point.Y - 1 == Y;
    public bool IsBelowThePoint(PointOnBoard point) => point.X == X && point.Y + 1 == Y;
    public bool IsOnLeftOfThePoint(PointOnBoard point) => point.X - 1 == X && point.Y == Y;
    public bool IsOnRightOfThePoint(PointOnBoard point) => point.X + 1 == X && point.Y == Y;
    public bool IsNextTo(PointOnBoard point) => IsAboveThePoint(point) || IsBelowThePoint(point) || IsOnLeftOfThePoint(point) || IsOnRightOfThePoint(point);
    public IEnumerable<PointOnBoard> GetPointsClaimedByThisPoint() => new[]
        {
            new PointOnBoard(X, Y),
            new PointOnBoard(X + 1, Y),
            new PointOnBoard(X - 1, Y),
            new PointOnBoard(X, Y + 1),
            new PointOnBoard(X, Y - 1),

            new PointOnBoard(X - 1, Y - 1),
            new PointOnBoard(X + 1, Y - 1),
            new PointOnBoard(X - 1, Y + 1),
            new PointOnBoard(X + 1, Y + 1),
        };

    public IEnumerable<PointOnBoard> GetPointsNextToThisPoint() => new[]
    {
        new PointOnBoard(X + 1, Y),
        new PointOnBoard(X - 1, Y),
        new PointOnBoard(X, Y + 1),
        new PointOnBoard(X, Y - 1),
    };

    public bool PointIsInSpecifiedSize(int minAvailableSize, int maxAvailableSize) =>
        X >= minAvailableSize && X <= maxAvailableSize && Y >= minAvailableSize && Y <= maxAvailableSize; 

    public override bool Equals(object? obj)
    {
        if (obj is PointOnBoard point)
            return point == this;

        return false;
    }

    public override int GetHashCode()
    {
        return (X.GetHashCode() + Y.GetHashCode()).GetHashCode();
    }
}