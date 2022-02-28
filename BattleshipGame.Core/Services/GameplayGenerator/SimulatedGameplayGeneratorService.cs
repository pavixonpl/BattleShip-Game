using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.ShotProvider;

namespace BattleshipGame.Core.Services.GameplayGenerator;

public class SimulatedGameplayGeneratorService : IGameplayGeneratorService
{
    private readonly IShootingService _shootingService;

    public SimulatedGameplayGeneratorService(IShootingService shootingService)
    {
        _shootingService = shootingService;
    }
    public MatchResult PlayMatch(GameBoardPattern boardPattern)
    {
        var currentPlayer = Player.First;

        var gameBoardWithPlayers = GameBoardWithPlayers.Create(boardPattern, Player.First, Player.Second);

        var round = 1;
        while (true)
        {
            var currentPlayerBoard = gameBoardWithPlayers.GetPlayer(currentPlayer);
            
            var shotFired = Shoot(currentPlayerBoard, round);
            currentPlayerBoard.AddShot(shotFired);

            if (GameIsOver(currentPlayerBoard))
                break;



            if (currentPlayer == Player.Second)
                round++;

            currentPlayer = GetOppositePlayer(currentPlayer);
        }

        return new MatchResult(gameBoardWithPlayers, currentPlayer);
    }

    public Shot Shoot(GameBoardWithPlayer board, int round)
    {
        var pointOfShot = _shootingService.GetPointToShot(board.Pattern, board.Shots);

        return Shot.Create(pointOfShot, board, round);
    }

    public bool GameIsOver(GameBoardWithPlayer board)
    {
        var shipDestroyed = board.Shots.MaxBy(s => s.Round).Result == ShotResult.HitAndSunk;

        if (shipDestroyed)
        {
            return board.Pattern.ShipsOnBoard.Sum(s => s.Ship.Length) == board.Shots.Count(s => s.Result is ShotResult.Hit or ShotResult.HitAndSunk);
        }

        return false;
    }

    private Player GetOppositePlayer(Player currentPlayer) =>
        currentPlayer == Player.First ? Player.Second : Player.First;
}