using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Domain.Gameplay;

public class GameBoardWithPlayer
{
    public GameBoardWithPlayer(Player player, GameBoardWithPlayers board, SingleBoardPattern pattern)
    {
        Player = player;
        Board = board;
        Pattern = pattern;
        _shots = new List<Shot>();
    }

    protected GameBoardWithPlayer()
    {

    }

    private readonly List<Shot> _shots;
    public IReadOnlyCollection<Shot> Shots => _shots;
    public Player Player { get; }
    public virtual GameBoardWithPlayers Board { get; }
    public virtual SingleBoardPattern Pattern { get; }

    public void AddShot(Shot shotToAdd)
    {
        if (Pattern.PointIsOverThePattern(shotToAdd.PointOfShot))
            throw new ArgumentException($"Given point has different X or Y then which is allowed on board Pattern, given X: {shotToAdd.PointOfShot.X}, given Y: {shotToAdd.PointOfShot.Y}");

        _shots.Add(shotToAdd);
    }
}