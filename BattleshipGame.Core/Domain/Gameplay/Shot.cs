using System.Runtime.CompilerServices;
using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Pattern;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace BattleshipGame.Core.Domain.Gameplay;

public class Shot
{
    public Shot(ShotResult result, GameBoardWithPlayer gameBoard, PointOnBoard pointOfShot, int round)
    {
        Result = result;
        GameBoard = gameBoard;
        PointOfShot = pointOfShot;
        Round = round;
    }

    protected Shot()
    {

    }

    public ShotResult Result { get; set; }
    public PointOnBoard PointOfShot { get; set; }
    public int Round { get; set; }
    public GameBoardWithPlayer GameBoard { get; set; }

    public static Shot Create(PointOnBoard pointOfShot, GameBoardWithPlayer gameBoard, int round)
    {
        var shotResult = GetShotResult(gameBoard.Board.GetOppositePlayer(gameBoard.Player).Pattern, pointOfShot, gameBoard.Shots);
        return new Shot(shotResult, gameBoard, pointOfShot, round);
    }

    public static ShotResult GetShotResult(SingleBoardPattern secondPlayersSingleBoard, PointOnBoard pointOfShot, IReadOnlyCollection<Shot> shotsOnThisBoard)
    {
        var shipOnPoint = secondPlayersSingleBoard.GetShipOnPoint(pointOfShot);
        if (shipOnPoint == null)
            return ShotResult.Miss;

        var shipHitPoints = new List<PointOnBoard>();

        foreach (var point in shipOnPoint.PointsOfShip)
        {
            var hitPoint = shotsOnThisBoard.FirstOrDefault(s => s.PointOfShot == point);
            if (hitPoint != null)
                shipHitPoints.Add(hitPoint.PointOfShot);
        }

        return shipHitPoints.Count == shipOnPoint.Ship.Length - 1 ? ShotResult.HitAndSunk : ShotResult.Hit;
    }
}