using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Services.BoardPatternGenerator;

public interface IBoardGeneratorService
{
    Task<SingleBoardPattern> GenerateSingleBoardAsync();
}

