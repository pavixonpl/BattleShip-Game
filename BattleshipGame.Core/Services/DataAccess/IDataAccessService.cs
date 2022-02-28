using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Services.DataAccess;

public interface IDataAccessService
{

    Task<IEnumerable<MatchResult>> GetMatchResultsByBoard(string boardId);
    Task<GameBoardPattern> GetBoardPattern(string boardId);
    Task<SingleBoardPattern> GetSingleBoardPattern(string singleBoardPatternId);
    Task<MatchResult> GetMatchResult(string matchId);
    Task AddBoard(GameBoardPattern newBoardPattern);
    Task AddMatchResult(MatchResult newMatch);
}