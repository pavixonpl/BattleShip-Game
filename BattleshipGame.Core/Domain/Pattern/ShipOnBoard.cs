namespace BattleshipGame.Core.Domain.Pattern;

public class ShipOnBoard
{
    public ShipOnBoard(Ship ship, PointOnBoard startPoint, PointOnBoard endPoint)
    {
        Ship = ship;
        SetBorderPointsOfShip(startPoint, endPoint);
        PointsOfShip = GetAllPointsOfShip();
    }

    private ShipOnBoard()
    {

    }
    private PointOnBoard _startPoint;
    private PointOnBoard _endPoint;

    public Ship Ship { get; }
    public IEnumerable<PointOnBoard> PointsOfShip { get; }

    public bool PointIsClaimedByShip(PointOnBoard point) 
        => PointsOfShip.SelectMany(d=>d.GetPointsClaimedByThisPoint()).Distinct().Any(s=>s == point);
    
    private IEnumerable<PointOnBoard> GetAllPointsOfShip() =>
        ShipIsVertical()
            ? Enumerable.Range(0, Ship.Length).Select(i => new PointOnBoard(_startPoint.X, _startPoint.Y + i))
            : Enumerable.Range(0, Ship.Length).Select(i => new PointOnBoard(_startPoint.X + i, _startPoint.Y));

    private static bool ShipIsVertical(PointOnBoard startPoint, PointOnBoard endPoint) => startPoint.X == endPoint.X;
    private static bool ShipIsHorizontal(PointOnBoard startPoint, PointOnBoard endPoint) => startPoint.Y == endPoint.Y;

    private bool ShipIsVertical() => ShipIsVertical(_startPoint, _endPoint);

    private void SetBorderPointsOfShip(PointOnBoard startPoint, PointOnBoard endPoint)
    {
        if (ShipIsVertical(startPoint, endPoint))
        {
            if (startPoint.Y < endPoint.Y)
            {
                _startPoint = startPoint;
                _endPoint = endPoint;
            }
            else
            {
                _startPoint = endPoint;
                _endPoint = startPoint;
            }
        }
        else if (ShipIsHorizontal(startPoint, endPoint))
        {
            if (startPoint.X < endPoint.X)
            {
                _startPoint = startPoint;
                _endPoint = endPoint;
            }
            else
            {
                _startPoint = endPoint;
                _endPoint = startPoint;
            }
        }
        else
            throw new ArgumentException($"Given points to create are for diagonal ship, which is not allowed. Given points: Start: X:{startPoint.X} Y:{startPoint.Y} End: X: {endPoint.X} Y:{endPoint.Y}");
    }
}