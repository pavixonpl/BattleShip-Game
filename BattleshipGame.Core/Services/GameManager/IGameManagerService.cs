using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Services.GameManager;

public interface IGameManagerService
{
    Task<GameBoardPattern> CreateGamesBoardAsync();
    Task<MatchResult> PlaySingleMatchAsync(string boardId);

}