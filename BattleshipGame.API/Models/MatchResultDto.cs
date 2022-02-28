using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.API.Models;

public class MatchResultDto
{
    public string Id { get; set; }
    public PlayersBoardDto Winner { get; set; }
    public PlayersBoardDto Loser { get; set; }
}

public class PlayersBoardDto
{
    public Player Player { get; set; }
    public IEnumerable<ShotDto> Shots { get; set; }
    public string SingleBoardPatternId { get; set; }
}

public class ShotDto
{
    public ShotResult Result { get; set; }
    public PointOnBoard Point { get; set; }
    public int Round { get; set; }
}