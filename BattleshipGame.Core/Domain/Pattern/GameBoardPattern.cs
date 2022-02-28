namespace BattleshipGame.Core.Domain.Pattern;

public class GameBoardPattern
{
    public GameBoardPattern(SingleBoardPattern firstPlayersBoard, SingleBoardPattern secondPlayersBoard)
    {
        if (!firstPlayersBoard.HasTheSameAssumptionsAs(secondPlayersBoard))
            throw new ArgumentException("Given boards has different assumptions.");

        FirstPlayersBoard = firstPlayersBoard;
        SecondPlayersBoard = secondPlayersBoard;
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public SingleBoardPattern FirstPlayersBoard { get; set; }
    public SingleBoardPattern SecondPlayersBoard { get; set; }
}