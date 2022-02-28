using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.DataAccess;

namespace BattleshipGame.DAL
{
    public class InMemoryDataAccessService : IDataAccessService
    {
        public InMemoryDataAccessService()
        {
            _matchResults = new List<MatchResult>();
            _gameBoards = new List<GameBoardPattern>();
        }
        private List<MatchResult> _matchResults { get; set; }
        private List<GameBoardPattern> _gameBoards { get; set; }
        public Task<IEnumerable<MatchResult>> GetMatchResultsByBoard(string boardId)
        {
            return Task.FromResult(_matchResults.Where(s => s.Board.BoardPattern.Id == boardId));
        }

        public Task<SingleBoardPattern> GetSingleBoardPattern(string singleBoardPatternId)
        {
            var board = _gameBoards.FirstOrDefault(s => s.SecondPlayersBoard.Id == singleBoardPatternId || s.FirstPlayersBoard.Id == singleBoardPatternId);
            return board == null ? null : Task.FromResult(board.FirstPlayersBoard.Id == singleBoardPatternId ? board.FirstPlayersBoard : board.SecondPlayersBoard);
        }

        public Task<GameBoardPattern> GetBoardPattern(string boardId)
        {
            return Task.FromResult(_gameBoards.FirstOrDefault(s => s.Id == boardId));
        }

        public Task<MatchResult> GetMatchResult(string matchId)
        {
            return Task.FromResult(_matchResults.FirstOrDefault(s => s.Id == matchId));
        }

        public Task AddBoard(GameBoardPattern newBoardPattern)
        {
            _gameBoards.Add(newBoardPattern);
            return Task.CompletedTask;
        }

        public Task AddMatchResult(MatchResult newMatch)
        {
            _matchResults.Add(newMatch);
            return Task.CompletedTask;
        }
    }
}
