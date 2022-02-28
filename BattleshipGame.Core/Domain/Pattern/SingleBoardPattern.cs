namespace BattleshipGame.Core.Domain.Pattern;

public class SingleBoardPattern
{
    public SingleBoardPattern(int lengthAndHeightOfBoard, IEnumerable<ShipOnBoard> shipsOnBoard)
    {
        Id = Guid.NewGuid().ToString();
        LengthAndHeightOfBoard = lengthAndHeightOfBoard;

        if (shipsOnBoard.DistinctBy(s => s.Ship.Name).Count() != shipsOnBoard.Count())
            throw new ArgumentException("Ships must have unique names");
        
        ShipsOnBoard = shipsOnBoard;
    }
    protected SingleBoardPattern()
    {

    }
    public string Id { get; }
    public virtual int LengthAndHeightOfBoard { get; }
    public virtual IEnumerable<ShipOnBoard> ShipsOnBoard { get; }

    public bool HasTheSameAssumptionsAs(SingleBoardPattern board)
    {
        if (LengthAndHeightOfBoard != board.LengthAndHeightOfBoard)
            return false;

        var ships = ShipsOnBoard.Select(s => s.Ship).OrderBy(s=>s.Name);
        var givenShips = board.ShipsOnBoard.Select(s => s.Ship).OrderBy(s => s.Name);

        return ships.SequenceEqual(givenShips);
    }

    public bool PointIsOverThePattern(PointOnBoard point)
    {
        if (point.X <= 0 || point.X > LengthAndHeightOfBoard)
            return false;

        return point.Y <= 0 || point.Y > LengthAndHeightOfBoard;
    }

    public ShipOnBoard GetShipOnPoint(PointOnBoard point)
    {
        return ShipsOnBoard.FirstOrDefault(ship => ship.PointsOfShip.Any(s => s == point));
    }
}