using BattleshipGame.API.Models;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.DataAccess;
using BattleshipGame.Core.Services.GameManager;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BattleshipGameController : ControllerBase
{
    private readonly IGameManagerService _gameManagerService;
    private readonly IDataAccessService _dataAccessService;

    public BattleshipGameController(IGameManagerService gameManagerService, IDataAccessService dataAccessService)
    {
        _gameManagerService = gameManagerService;
        _dataAccessService = dataAccessService;
    }

    [HttpGet("singleBoardPattern/{singleBoardPatternId}")]
    public async Task<ActionResult<SingleBoardPattern>> GetSingleBoardPattern([FromRoute] string singleBoardPatternId)
    {
        var result = await _dataAccessService.GetSingleBoardPattern(singleBoardPatternId);
        return Ok(result);
    }

    [HttpGet("boardPattern/{boardId}")]
    public async Task<ActionResult<GameBoardPattern>> GetGameBoard([FromRoute] string boardId)
    {
        var result = await _dataAccessService.GetBoardPattern(boardId);
        return Ok(result);
    }

    [HttpPost("boardPattern")]
    public async Task<ActionResult<GameBoardPattern>> CreateGameBoard()
    {
        var result = await _gameManagerService.CreateGamesBoardAsync();
        return CreatedAtAction(nameof(GetGameBoard), new { boardId = result }, result);
    }

    [HttpGet("boardPattern/{boardId}/match/{matchId}")]
    public async Task<ActionResult<MatchResultDto>> GetMatch([FromRoute] string boardId, [FromRoute] string matchId)
    {
        var result = await _dataAccessService.GetMatchResult(matchId);
        return Ok(MapToDto(result));
    }

    [HttpPost("boardPattern/{boardId}/match")]
    public async Task<ActionResult<MatchResultDto>> PlaySingleMatch([FromRoute] string boardId)
    {
        var result = await _gameManagerService.PlaySingleMatchAsync(boardId);
        return CreatedAtAction(nameof(GetMatch), new { boardId = result.Board.BoardPattern.Id, matchId = result.Id }, MapToDto(result));
    }

    private MatchResultDto MapToDto(MatchResult result) => new MatchResultDto()
    {
        Id = result.Id,
        Loser = new PlayersBoardDto()
        {
            Player = result.Board.GetOppositePlayer(result.Winner).Player,
            Shots = result.Board.GetOppositePlayer(result.Winner).Shots.Select(MapToShotDto),
            SingleBoardPatternId = result.Board.GetOppositePlayer(result.Winner).Pattern.Id
        },
        Winner = new PlayersBoardDto()
        {
            Player = result.Board.GetPlayer(result.Winner).Player,
            Shots = result.Board.GetPlayer(result.Winner).Shots.Select(MapToShotDto),
            SingleBoardPatternId = result.Board.GetPlayer(result.Winner).Pattern.Id
        },
    };

    private ShotDto MapToShotDto(Shot shot) => new ShotDto()
    {
        Point = shot.PointOfShot,
        Result = shot.Result,
        Round = shot.Round
    };
}