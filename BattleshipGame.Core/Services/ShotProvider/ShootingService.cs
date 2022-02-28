using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using Microsoft.VisualBasic;

namespace BattleshipGame.Core.Services.ShotProvider;

public class ShootingService : IShootingService
{
    public PointOnBoard GetPointToShot(SingleBoardPattern boardPattern, IReadOnlyCollection<Shot> shotsFired)
    {
        //Shoots until point is find
        while (true)
        {
            //Firstly get point if ship is currently under fire (2 or more hits from last destroyed ship)
            var pointToShot = GetNextPointOfShipUnderFire(shotsFired);
            if (pointToShot != null)
                return pointToShot;

            //Secondly gets point next to single shot which was fired, excluding points claimed by others ships/ already shot.
            pointToShot = GetNextPointAfterFirstHitInShip(shotsFired);
            if (pointToShot != null)
                return pointToShot;

            //At last step will get point randomly selected from board
            //if points was not shot already/ is not claimed by any hit ship
            //or there is a possibility of existing ship there (not surrounded by points mentioned earlier/ enough space for not destroyed ships)
            return GetRandomPoint(boardPattern.LengthAndHeightOfBoard, shotsFired);
        }
    }

    public PointOnBoard GetNextPointAfterFirstHitInShip(IReadOnlyCollection<Shot> shotsFired)
    {
        var lastHit = GetLastHitToShip(shotsFired);

        if (lastHit == null)
            return null; //No hits in game

        var lastDestroyedShip = shotsFired.LastOrDefault(d => d.Result == ShotResult.HitAndSunk);

        if (lastDestroyedShip != null && lastDestroyedShip.Round > lastHit.Round)
            return null;

        var pointsClaimedByDestroyedShips = shotsFired.Where(s => (s.Round < lastHit.Round && s.Result is ShotResult.Hit) || s.Result is ShotResult.HitAndSunk).SelectMany(d => d.PointOfShot.GetPointsClaimedByThisPoint());
        var missedShots = shotsFired.Where(s => s.Result is ShotResult.Miss).Select(d => d.PointOfShot);
        var pointsClaimedByLastHit = lastHit.PointOfShot.GetPointsNextToThisPoint();

        //Get points to which shot was not fired and they are not claimed by destroyed ships
        var possibilitiesToShot = pointsClaimedByLastHit.Except(missedShots).Except(pointsClaimedByDestroyedShips).Except(new[] { lastHit.PointOfShot });

        var boardLength = shotsFired.First().GameBoard.Pattern.LengthAndHeightOfBoard;

        //Remove all points which are out of the border of board
        possibilitiesToShot = possibilitiesToShot.Where(s => s.PointIsInSpecifiedSize(1, boardLength));

        //Returns randomly selected point from this collection of possibilities to shot
        return possibilitiesToShot.Any() ? possibilitiesToShot.ElementAt(new Random().Next(0, possibilitiesToShot.Count())) : null;
    }

    public PointOnBoard GetNextPointOfShipUnderFire(IReadOnlyCollection<Shot> shotsFired)
    {
        var lastHitToShip = GetLastHitToShip(shotsFired);

        if (lastHitToShip == null)
            return null; //No hits in game

        var hitNextToLastHit = shotsFired.FirstOrDefault(d => d.PointOfShot.IsNextTo(lastHitToShip.PointOfShot) && d.Result == ShotResult.Hit);

        if (hitNextToLastHit == null)
            return null; //Ship was hit only once

        var pointsOfShip = new[] { lastHitToShip.PointOfShot, hitNextToLastHit.PointOfShot };

        var xAxisIsCommon = pointsOfShip.Select(s => s.X).Distinct().Count() == 1;
        var areNextToEachOtherOnY = pointsOfShip.ElementAt(0).Y - pointsOfShip.ElementAt(1).Y is -1 or 1;

        if (xAxisIsCommon && areNextToEachOtherOnY)
        {
            var allHitsToThisShip = GetAllHitsToShipOnXAxis(pointsOfShip, shotsFired);

            if (IsShipDestroyed(shotsFired, allHitsToThisShip))
                return null; //Ship is already destroyed, no need to proceed 

            var boardLength = shotsFired.First().GameBoard.Pattern.LengthAndHeightOfBoard;

            var shotPossibilities = new[] { GetPointToShotAbove, GetPointToShotBelow };
            var randomIndex = new Random().Next(0, 2);

            //Gets point in randomly selected direction of X axis to which shot will be fired
            var pointToShot = shotPossibilities[randomIndex].Invoke(shotsFired, allHitsToThisShip, boardLength);
            if (pointToShot != null)
                return pointToShot;

            //If shot could not be fired, that used another direction
            pointToShot = shotPossibilities[randomIndex == 0 ? 1 : 0].Invoke(shotsFired, allHitsToThisShip, boardLength);
            if (pointToShot != null)
                return pointToShot;

        }

        var yAxisIsCommon = pointsOfShip.Select(s => s.Y).Distinct().Count() == 1;
        var areNextToEachOtherOnX = pointsOfShip.ElementAt(0).X - pointsOfShip.ElementAt(1).X is -1 or 1;

        if (yAxisIsCommon && areNextToEachOtherOnX)
        {
            var allHitsToThisShip = GetAllHitsToShipOnYAxis(pointsOfShip, shotsFired);

            if (IsShipDestroyed(shotsFired, allHitsToThisShip))
                return null; //Ship is already destroyed, no need to proceed 

            var boardLength = shotsFired.First().GameBoard.Pattern.LengthAndHeightOfBoard;

            var shotPossibilities = new[] { GetPointToShotOnLeft, GetPointToShotOnRight };
            var randomIndex = new Random().Next(0, 2);

            //Gets point in randomly selected direction of Y axis to which shot will be fired
            var pointToShot = shotPossibilities[randomIndex].Invoke(shotsFired, allHitsToThisShip, boardLength);
            if (pointToShot != null)
                return pointToShot;

            //If shot could not be fired, that used another direction
            pointToShot = shotPossibilities[randomIndex == 0 ? 1 : 0].Invoke(shotsFired, allHitsToThisShip, boardLength);
            if (pointToShot != null)
                return pointToShot;

        }

        return null; //First hit in ship
    }


    public PointOnBoard GetRandomPoint(int boardLength, IReadOnlyCollection<Shot> shotsFired)
    {
        var random = new Random();
        while (true)
        {
            var randomX = random.Next(1, boardLength + 1);
            var randomY = random.Next(1, boardLength + 1);
            var randomPoint = new PointOnBoard(randomX, randomY);

            if (!shotsFired.Any())
                return randomPoint;

            //Point was hit in past
            if (shotsFired.Select(s => s.PointOfShot).Contains(randomPoint))
                continue;

            //Point can be ship, means is not surrounded by ships/ miss shots
            //Also have enough distance to be smallest not destroyed ship
            if (CanPointBeShip(randomPoint, shotsFired, boardLength))
                return randomPoint;
        }
    }

    protected bool CanPointBeShip(PointOnBoard randomPoint, IEnumerable<Shot> shotsFired, int boardLength)
    {
        var pointsWithHit = shotsFired.Where(s => s.Result is ShotResult.Hit or ShotResult.HitAndSunk);

        var claimedPoints = pointsWithHit.SelectMany(s => s.PointOfShot.GetPointsClaimedByThisPoint()).Where(s => s.PointIsInSpecifiedSize(1, boardLength)).Distinct();

        //Point is claimed by ship which was previously fired
        if (claimedPoints.Any(s => s == randomPoint))
            return false;

        var pointsNextToRandom = randomPoint.GetPointsNextToThisPoint().Where(s => s.PointIsInSpecifiedSize(1, boardLength));

        var pointCanBeShip = pointsNextToRandom.Except(claimedPoints).Except(shotsFired.Select(s => s.PointOfShot)).Any();

        if (!pointCanBeShip)
            return false;

        var othersPlayerBoard = shotsFired.First().GameBoard.Board.GetOppositePlayer(shotsFired.First().GameBoard.Player).Pattern;

        //Get ships which were hit by checking on other players board
        var destroyedShips = pointsWithHit.Select(s => othersPlayerBoard.GetShipOnPoint(s.PointOfShot)).Select(d => d.Ship).Distinct();

        //Get ship sizes on ships not destroyed yet
        var possibleShipSizes = othersPlayerBoard.ShipsOnBoard.Select(s => s.Ship).Except(destroyedShips).Select(d => d.Length).Distinct();

        //All points to which could not be fired
        var allUsedPoints = claimedPoints.Concat(shotsFired.Select(s => s.PointOfShot)).Distinct();

        //Gets distance in every direction from random to claimed points
        var distances = GetDistancesToClaimedPoints(randomPoint, allUsedPoints, boardLength);

        return distances.Any(s => s >= possibleShipSizes.Min());
    }

    protected IReadOnlyCollection<int> GetDistancesToClaimedPoints(PointOnBoard randomPoint, IEnumerable<PointOnBoard> allUsedPoints, int boardLength)
    {
        var leftestPoint = allUsedPoints.Where(d => d.Y == randomPoint.Y).MinBy(s => s.X - randomPoint.X);
        var rightestPoint = allUsedPoints.Where(d => d.Y == randomPoint.Y).MaxBy(s => s.X + randomPoint.X);
        var lowestPoint = allUsedPoints.Where(d => d.X == randomPoint.X).MaxBy(s => s.Y + randomPoint.Y);
        var highestPoint = allUsedPoints.Where(d => d.X == randomPoint.X).MinBy(s => s.Y - randomPoint.Y);

        //Distance to borders
        var distanceToLeft = randomPoint.X;
        var distanceToRight = boardLength - randomPoint.X + 1;
        var distanceAbove = randomPoint.Y;
        var distanceBelow = boardLength - randomPoint.Y + 1;

        //Point is on left site of random
        if (leftestPoint is not null && randomPoint.X > leftestPoint.X)
        {
            //Right point is closer on left then leftest point
            if (randomPoint.X > rightestPoint.X)
                distanceToLeft = randomPoint.X - rightestPoint.X;
            else
                distanceToLeft = randomPoint.X - leftestPoint.X; 
        }

        //Point is on right site of random
        if (rightestPoint is not null && rightestPoint.X > randomPoint.X)
        {
            //Left point is closer on right then rightest point
            if (randomPoint.X < leftestPoint.X)
                distanceToRight = leftestPoint.X - randomPoint.X;
            else
                distanceToRight = rightestPoint.X - randomPoint.X;
        }

        //Point is on above random
        if (highestPoint is not null && highestPoint.Y < randomPoint.Y)
        {
            //Lowest point is closer on high then highest point
            if (randomPoint.Y < lowestPoint.Y)
                distanceAbove = lowestPoint.Y - randomPoint.Y;
            else
                distanceAbove = highestPoint.Y - randomPoint.Y;
        }

        //Point is on below random
        if (lowestPoint is not null && lowestPoint.Y > randomPoint.Y)
        {
            //Highest point is closer on low then lowest point
            if (randomPoint.Y > highestPoint.Y)
                distanceBelow = randomPoint.Y - highestPoint.Y;
            else
                distanceBelow = randomPoint.Y - lowestPoint.Y;
        }

        return new[] { distanceBelow, distanceAbove, distanceToLeft, distanceToRight };
    }

    protected Func<IEnumerable<Shot>, IEnumerable<PointOnBoard>, int, PointOnBoard> GetPointToShotOnRight = (shotsFired, allHitsToThisShip, boardLength) =>
    {
        var pointWasNeverHit = shotsFired.FirstOrDefault(s => s.PointOfShot.IsOnRightOfThePoint(allHitsToThisShip.MaxBy(s => s.X)) && s.Result == ShotResult.Miss) == null;
        var pointOfShot = new PointOnBoard(allHitsToThisShip.MaxBy(s => s.X).X + 1, allHitsToThisShip.MaxBy(s => s.X).Y);

        if (pointWasNeverHit && pointOfShot.PointIsInSpecifiedSize(1, boardLength))
        {
            var nextToPoints = pointOfShot.GetPointsNextToThisPoint().Except(new[] { allHitsToThisShip.MaxBy(s => s.X) });
            var pointsClaimedByAnotherShips = shotsFired.Where(s => nextToPoints.Any(d => d == s.PointOfShot));

            //If any of next to points is ship then method returns null
            if (!pointsClaimedByAnotherShips.Any() || pointsClaimedByAnotherShips.All(s => s.Result == ShotResult.Miss))
                return pointOfShot;
        }

        return null;
    };

    protected Func<IEnumerable<Shot>, IEnumerable<PointOnBoard>, int, PointOnBoard> GetPointToShotOnLeft = (shotsFired, allHitsToThisShip, boardLength) =>
    {
        var pointWasNeverHit = shotsFired.FirstOrDefault(s => s.PointOfShot.IsOnLeftOfThePoint(allHitsToThisShip.MinBy(s => s.X)) && s.Result == ShotResult.Miss) == null;
        var pointOfShot = new PointOnBoard(allHitsToThisShip.MinBy(s => s.X).X - 1, allHitsToThisShip.MinBy(s => s.X).Y);

        if (pointWasNeverHit && pointOfShot.PointIsInSpecifiedSize(1, boardLength))
        {
            var nextToPoints = pointOfShot.GetPointsNextToThisPoint().Except(new[] { allHitsToThisShip.MinBy(s => s.X) });
            var pointsClaimedByAnotherShips = shotsFired.Where(s => nextToPoints.Any(d => d == s.PointOfShot));

            //If any of next to points is ship then method returns null
            if (!pointsClaimedByAnotherShips.Any() || pointsClaimedByAnotherShips.All(s => s.Result == ShotResult.Miss))
                return pointOfShot;
        }

        return null;
    };

    protected Func<IEnumerable<Shot>, IEnumerable<PointOnBoard>, int, PointOnBoard> GetPointToShotAbove = (shotsFired, allHitsToThisShip, boardLength) =>
    {
        var pointWasNeverHit = shotsFired.FirstOrDefault(s => s.PointOfShot.IsAboveThePoint(allHitsToThisShip.MinBy(s => s.Y)) && s.Result == ShotResult.Miss) == null;
        var pointOfShot = new PointOnBoard(allHitsToThisShip.MinBy(s => s.Y).X, allHitsToThisShip.MinBy(s => s.Y).Y - 1);

        if (pointWasNeverHit && pointOfShot.PointIsInSpecifiedSize(1, boardLength))
        {
            var nextToPoints = pointOfShot.GetPointsNextToThisPoint().Except(new[] { allHitsToThisShip.MinBy(s => s.Y) });
            var pointsClaimedByAnotherShips = shotsFired.Where(s => nextToPoints.Any(d => d == s.PointOfShot));

            //If any of next to points is ship then method returns null
            if (!pointsClaimedByAnotherShips.Any() || pointsClaimedByAnotherShips.All(s => s.Result == ShotResult.Miss))
                return pointOfShot;
        }

        return null;
    };

    protected Func<IEnumerable<Shot>, IEnumerable<PointOnBoard>, int, PointOnBoard> GetPointToShotBelow = (shotsFired, allHitsToThisShip, boardLength) =>
    {
        var pointWasNeverHit = shotsFired.FirstOrDefault(s => s.PointOfShot.IsBelowThePoint(allHitsToThisShip.MaxBy(s => s.Y)) && s.Result == ShotResult.Miss) == null;
        var pointOfShot = new PointOnBoard(allHitsToThisShip.MaxBy(s => s.Y).X, allHitsToThisShip.MaxBy(s => s.Y).Y + 1);

        if (pointWasNeverHit && pointOfShot.PointIsInSpecifiedSize(1, boardLength))
        {
            var nextToPoints = pointOfShot.GetPointsNextToThisPoint().Except(new[] { allHitsToThisShip.MaxBy(s => s.Y) });
            var pointsClaimedByAnotherShips = shotsFired.Where(s => nextToPoints.Any(d => d == s.PointOfShot));

            //If any of next to points is ship then method returns null
            if (!pointsClaimedByAnotherShips.Any() || pointsClaimedByAnotherShips.All(s => s.Result == ShotResult.Miss))
                return pointOfShot;
        }

        return null;
    };

    protected IReadOnlyCollection<PointOnBoard> GetAllHitsToShipOnXAxis(IEnumerable<PointOnBoard> hitsToShip, IReadOnlyCollection<Shot> allShotsFired)
    {
        var allHitsOnXAxis = allShotsFired.Where(s => s.Result is ShotResult.Hit or ShotResult.HitAndSunk && s.PointOfShot.X == hitsToShip.First().X);

        if (allHitsOnXAxis.DistinctBy(s => s.PointOfShot.X).Count() > 1)
            throw new InvalidOperationException("Given hits on x axis, don't have common X");

        //Go through every hit and check is it a direct part current ship
        var allHitsToThisShip = new List<PointOnBoard>(hitsToShip);
        for (var i = 0; i < allHitsOnXAxis.Count(); i++)
        {
            var upperPoint = allHitsOnXAxis.FirstOrDefault(s => s.PointOfShot.IsAboveThePoint(allHitsToThisShip.MinBy(s => s.Y)));

            if (upperPoint != null)
                allHitsToThisShip.Add(upperPoint.PointOfShot);

            var bottomPoint = allHitsOnXAxis.FirstOrDefault(s => s.PointOfShot.IsBelowThePoint(allHitsToThisShip.MaxBy(s => s.Y)));

            if (bottomPoint != null)
                allHitsToThisShip.Add(bottomPoint.PointOfShot);
        }

        return allHitsToThisShip.Distinct().ToList();
    }

    protected IReadOnlyCollection<PointOnBoard> GetAllHitsToShipOnYAxis(IEnumerable<PointOnBoard> hitsToShip, IReadOnlyCollection<Shot> allShotsFired)
    {
        var allHitsOnYAxis = allShotsFired.Where(s => s.Result is ShotResult.Hit or ShotResult.HitAndSunk && s.PointOfShot.Y == hitsToShip.First().Y);

        if (allHitsOnYAxis.DistinctBy(s => s.PointOfShot.Y).Count() > 1)
            throw new InvalidOperationException("Given hits on y axis, don't have common Y");

        //Go through every hit and check is it a direct part current ship
        var allHitsToThisShip = new List<PointOnBoard>(hitsToShip);
        for (var i = 0; i < allHitsOnYAxis.Count(); i++)
        {
            var leftPoint = allHitsOnYAxis.FirstOrDefault(s => s.PointOfShot.IsOnLeftOfThePoint(allHitsToThisShip.MinBy(s => s.X)));

            if (leftPoint != null)
                allHitsToThisShip.Add(leftPoint.PointOfShot);

            var rightPoint = allHitsOnYAxis.FirstOrDefault(s => s.PointOfShot.IsOnRightOfThePoint(allHitsToThisShip.MaxBy(s => s.X)));

            if (rightPoint != null)
                allHitsToThisShip.Add(rightPoint.PointOfShot);
        }

        return allHitsToThisShip.Distinct().ToList();
    }

    protected bool IsShipDestroyed(IEnumerable<Shot> allShots, IEnumerable<PointOnBoard> pointsOfShip) => allShots.Any(s => s.Result == ShotResult.HitAndSunk && pointsOfShip.Any(d => d == s.PointOfShot));

    protected Shot GetLastHitToShip(IEnumerable<Shot> shots) => shots.OrderBy(s => s.Round).LastOrDefault(d => d.Result == ShotResult.Hit);

}