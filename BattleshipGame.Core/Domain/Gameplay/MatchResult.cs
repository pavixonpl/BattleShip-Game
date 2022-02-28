using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Domain.Gameplay;

public class MatchResult
{
    public MatchResult(GameBoardWithPlayers board, Player winner)
    {
        Id = Guid.NewGuid().ToString();
        Board = board;
        Winner = winner;
    }

    public string Id { get; set; }
    public GameBoardWithPlayers Board { get; }
    public Player Winner { get; }
}