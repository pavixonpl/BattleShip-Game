using System.Runtime.InteropServices;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.DataAccess;

namespace BattleshipGame.Core.Services.BoardPatternGenerator;

public class ClassicBoardGeneratorService : IBoardGeneratorService
{
    protected const int ClassicBoardStart = 1;
    protected const int ClassicBoardEnd = 10;
    protected Func<PointOnBoard, Ship, IEnumerable<ShipOnBoard>, PointOnBoard>[] _possibleDirectionsToCreateShip;
    public ClassicBoardGeneratorService()
    {
        _possibleDirectionsToCreateShip = new[] { GetShipGoingUpEndpoint, GetShipGoingDownEndpoint, GetShipGoingLeftEndpoint, GetShipGoingRightEndpoint };
    }
    public Task<SingleBoardPattern> GenerateSingleBoardAsync()
    {
        var ships = Ship.GetClassicShips();
        var shipsOnBoard = new List<ShipOnBoard>();

        foreach (var ship in ships)
        {
            shipsOnBoard.Add(GetShipOnBoardWithRandomLocation(ship, shipsOnBoard));
        }

        return Task.FromResult(new SingleBoardPattern(ClassicBoardEnd, shipsOnBoard));
    }

    private ShipOnBoard GetShipOnBoardWithRandomLocation(Ship ship, IEnumerable<ShipOnBoard> alreadyExistingShipsOnBoard)
    {
        PointOnBoard startPoint = null;
        PointOnBoard endPoint = null;

        while (startPoint is null)
        {
            startPoint = GetShipStartPoint();

            endPoint = GetShipEndPoint(startPoint, ship, alreadyExistingShipsOnBoard);

            if (endPoint == null)
                startPoint = null;
        }
        return new ShipOnBoard(ship, startPoint, endPoint);
    }

    private PointOnBoard GetShipEndPoint(PointOnBoard startPoint, Ship ship, IEnumerable<ShipOnBoard> alreadyExistingShipsOnBoard)
    {

        var r = new Random();

        var usedPossibilities = new List<int>();

        while (usedPossibilities.Count != _possibleDirectionsToCreateShip.Length)
        {
            //Randomly selects direction in which ship will be created in range
            var indexOfPossibility = r.Next(0, _possibleDirectionsToCreateShip.Length);

            //If direction was used then return to random selection
            if (usedPossibilities.Contains(indexOfPossibility))
                continue;

            //Gets endpoint for random direction
            var endPoint = _possibleDirectionsToCreateShip[indexOfPossibility].Invoke(startPoint, ship, alreadyExistingShipsOnBoard);

            //If endpoint is null then it means that creating ship is not allowed in selected direction
            if (endPoint != null)
                return endPoint;

            //Direction is added to already used directions
            usedPossibilities.Add(indexOfPossibility);
        }

        //Returns null if creating ship is not possible for given start point
        return null;
    }


    private PointOnBoard GetShipStartPoint() => new PointOnBoard(GetRandomClassicBoardNumber(), GetRandomClassicBoardNumber());
    private int GetRandomClassicBoardNumber() => new Random().Next(ClassicBoardStart, ClassicBoardEnd + 1);

    #region Ship creation directions
    
    public readonly Func<PointOnBoard, Ship, IEnumerable<ShipOnBoard>, PointOnBoard> GetShipGoingLeftEndpoint = (startPoint, ship, alreadyExistingShipsOnBoard) =>
    {
        var shipCanGoLeft = startPoint.X - ship.Length + 1 >= ClassicBoardStart;

        if (!shipCanGoLeft)
            return null;

        if (!alreadyExistingShipsOnBoard.Any())
            return new PointOnBoard(startPoint.X - ship.Length + 1, startPoint.Y);

        var pointsForNewShip = Enumerable.Range(0, ship.Length).Select(i => new PointOnBoard(startPoint.X - i, startPoint.Y));

        var noneOfPointsIsClaimedByExistingShips = pointsForNewShip.All(point => !alreadyExistingShipsOnBoard.Any(s => s.PointIsClaimedByShip(point)));

        return noneOfPointsIsClaimedByExistingShips ? pointsForNewShip.MinBy(s => s.X) : null;
    };

    public readonly Func<PointOnBoard, Ship, IEnumerable<ShipOnBoard>, PointOnBoard> GetShipGoingRightEndpoint = (startPoint, ship, alreadyExistingShipsOnBoard) =>
    {
        var shipCanGoRight = startPoint.X + ship.Length -1 <= ClassicBoardEnd;

        if (!shipCanGoRight)
            return null;

        if (!alreadyExistingShipsOnBoard.Any())
            return new PointOnBoard(startPoint.X + ship.Length - 1, startPoint.Y);

        var pointsForNewShip = Enumerable.Range(0, ship.Length).Select(i => new PointOnBoard(startPoint.X + i, startPoint.Y));

        var noneOfPointsIsClaimedByExistingShips = pointsForNewShip.All(point => !alreadyExistingShipsOnBoard.Any(s => s.PointIsClaimedByShip(point)));

        return noneOfPointsIsClaimedByExistingShips ? pointsForNewShip.MaxBy(s => s.X) : null;
    };

    public readonly Func<PointOnBoard, Ship, IEnumerable<ShipOnBoard>, PointOnBoard> GetShipGoingDownEndpoint = (startPoint, ship, alreadyExistingShipsOnBoard) =>
    {
        var shipCanGoDown = startPoint.Y + ship.Length - 1 <= ClassicBoardEnd;

        if (!shipCanGoDown)
            return null;

        if (!alreadyExistingShipsOnBoard.Any())
            return new PointOnBoard(startPoint.X, startPoint.Y + ship.Length - 1);

        var pointsForNewShip = Enumerable.Range(0, ship.Length).Select(i => new PointOnBoard(startPoint.X, startPoint.Y + i));

        var noneOfPointsIsClaimedByExistingShips = pointsForNewShip.All(point => !alreadyExistingShipsOnBoard.Any(s => s.PointIsClaimedByShip(point)));

        return noneOfPointsIsClaimedByExistingShips ? pointsForNewShip.MaxBy(s => s.Y) : null;
    };

    public readonly Func<PointOnBoard, Ship, IEnumerable<ShipOnBoard>, PointOnBoard> GetShipGoingUpEndpoint = (startPoint, ship, alreadyExistingShipsOnBoard) =>
    {
        var shipCanGoUp = startPoint.Y - ship.Length +1 >= ClassicBoardStart;

        if (!shipCanGoUp)
            return null;

        if (!alreadyExistingShipsOnBoard.Any())
            return new PointOnBoard(startPoint.X, startPoint.Y - ship.Length + 1);

        var pointsForNewShip = Enumerable.Range(0, ship.Length).Select(i => new PointOnBoard(startPoint.X, startPoint.Y - i)).ToList();

        var noneOfPointsIsClaimedByExistingShips = pointsForNewShip.All(point => !alreadyExistingShipsOnBoard.Any(s => s.PointIsClaimedByShip(point)));

        return noneOfPointsIsClaimedByExistingShips ? pointsForNewShip.MinBy(s => s.Y) : null;
    };

    #endregion
}