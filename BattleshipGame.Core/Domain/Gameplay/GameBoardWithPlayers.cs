using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Domain.Gameplay;

public class GameBoardWithPlayers
{
    //Idea: Instead of GameBoardWithPlayers it could be object of property of GameBoardWithPlayer in GameBoardWithPlayer OppositePlayer
    private GameBoardWithPlayers(GameBoardPattern boardPattern)
    {
        BoardPattern = boardPattern;
    }

    protected GameBoardWithPlayers()
    {

    }
    public GameBoardPattern BoardPattern { get; }
    private GameBoardWithPlayer PlayerOne { get; set; }
    private GameBoardWithPlayer PlayerTwo { get; set; }
    public GameBoardWithPlayer GetPlayer(Player player)
    {
        return player == PlayerOne.Player ? PlayerOne : PlayerTwo;
    }
    public virtual GameBoardWithPlayer GetOppositePlayer(Player player)
    {
        return player == PlayerOne.Player ? PlayerTwo : PlayerOne;
    }
    public static GameBoardWithPlayers Create(GameBoardPattern boardPattern, Player playerOne, Player playerTwo)
    {
        var gameBoard = new GameBoardWithPlayers(boardPattern);
        gameBoard.PlayerOne = new GameBoardWithPlayer(playerOne, gameBoard, boardPattern.FirstPlayersBoard);
        gameBoard.PlayerTwo = new GameBoardWithPlayer(playerTwo, gameBoard, boardPattern.SecondPlayersBoard);
        return gameBoard;
    }
}