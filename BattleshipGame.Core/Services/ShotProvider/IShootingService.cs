using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;

namespace BattleshipGame.Core.Services.ShotProvider
{
    public interface IShootingService
    {
        PointOnBoard GetPointToShot(SingleBoardPattern boardPattern, IReadOnlyCollection<Shot> shotsFired);
    }
}
