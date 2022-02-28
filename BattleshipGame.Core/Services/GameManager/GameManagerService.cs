using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Exceptions;
using BattleshipGame.Core.Services.BoardPatternGenerator;
using BattleshipGame.Core.Services.DataAccess;
using BattleshipGame.Core.Services.GameplayGenerator;

namespace BattleshipGame.Core.Services.GameManager;

public class GameManagerService : IGameManagerService
{
    private readonly IBoardGeneratorService _boardGeneratorService;
    private readonly IDataAccessService _dataAccessService;
    private readonly IGameplayGeneratorService _gameplayGenerator;

    public GameManagerService(IBoardGeneratorService boardGeneratorService, IDataAccessService dataAccessService, IGameplayGeneratorService gameplayGenerator)
    {
        _boardGeneratorService = boardGeneratorService;
        _dataAccessService = dataAccessService;
        _gameplayGenerator = gameplayGenerator;
    }
    public async Task<GameBoardPattern> CreateGamesBoardAsync()
    {
        var firstPlayersBoardPattern = await _boardGeneratorService.GenerateSingleBoardAsync();

        var secondPlayersBoardPattern = await _boardGeneratorService.GenerateSingleBoardAsync();

        var gameBoard = new GameBoardPattern(firstPlayersBoardPattern, secondPlayersBoardPattern);
        
        await _dataAccessService.AddBoard(gameBoard);
        return gameBoard;
    }

    public async Task<MatchResult> PlaySingleMatchAsync(string boardId)
    {
        var board = await _dataAccessService.GetBoardPattern(boardId);
        if (board is null)
            throw new NotFoundException($"BoardPattern with id: {boardId} was not found");

        var matchResult = _gameplayGenerator.PlayMatch(board);
        await _dataAccessService.AddMatchResult(matchResult);
        return matchResult;
    }
}