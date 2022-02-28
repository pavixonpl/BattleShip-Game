using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.ShotProvider;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BattleshipGame.Tests.Unit.Core.Services.Shooting
{
    public class GetRandomPointTests : ShootingService
    {
        [Test]
        public void OneFieldAvailableToShot_NoPlaceForShip_CanPointBeShipMethod_ReturnsFalse()
        {
            //Arrange
            var board = new Mock<GameBoardWithPlayer>();
            board.Setup(d => d.Board.GetOppositePlayer(It.IsAny<Player>()).Pattern.ShipsOnBoard).Returns(GetShipsOnBoard);

            var shotsFired = GetShotsFired();
            var randomPoint = new PointOnBoard(1, 1);
            var boardLength = 3;

            //Act
            var pointCanBeShip = CanPointBeShip(randomPoint, shotsFired, boardLength);

            //Assert
            pointCanBeShip.Should().BeFalse();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(2, 3), 1),
                new Shot(ShotResult.HitAndSunk, board.Object, new PointOnBoard(3, 3), 2),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(3, 2), 3),
            };

            ShipOnBoard[] GetShipsOnBoard() => new[]
            {
                new ShipOnBoard(new Ship("1", 2), new PointOnBoard(2, 3), new PointOnBoard(3, 3)),
            };
            #endregion
        }

        [Test]
        public void TwoFieldAvailableToShoot_ShipIsToBig_CanPointBeShipMethod_ReturnsFalse()
        {
            //Arrange
            var board = new Mock<GameBoardWithPlayer>();
            board.Setup(d => d.Board.GetOppositePlayer(It.IsAny<Player>()).Pattern.ShipsOnBoard).Returns(GetShipsOnBoard);

            var shotsFired = GetShotsFired();
            var randomPoint = new PointOnBoard(1, 1);
            var boardLength = 5;

            //Act
            var pointCanBeShip = CanPointBeShip(randomPoint, shotsFired, boardLength);

            //Assert
            pointCanBeShip.Should().BeFalse();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(4, 1), 1),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(5, 1), 2),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(1, 3), 3),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(2, 3), 4),
                new Shot(ShotResult.HitAndSunk, board.Object, new PointOnBoard(3, 3), 5),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(4, 3), 6),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(5, 3), 7),
            };

            ShipOnBoard[] GetShipsOnBoard() => new[]
            {
                new ShipOnBoard(new Ship("1", 2), new PointOnBoard(4, 1), new PointOnBoard(5, 1)),
                new ShipOnBoard(new Ship("2", 5), new PointOnBoard(1, 3), new PointOnBoard(5, 3)),
                new ShipOnBoard(new Ship("3", 5), new PointOnBoard(1, 5), new PointOnBoard(5, 5)),
            };

            #endregion
        }

        [Test]
        public void TwoFieldAvailableToShoot_ShipFitsInSize_CanPointBeShipMethod_ReturnsTrue()
        {
            //Arrange
            var board = new Mock<GameBoardWithPlayer>();
            board.Setup(d => d.Board.GetOppositePlayer(It.IsAny<Player>()).Pattern.ShipsOnBoard).Returns(GetShipsOnBoard);

            var shotsFired = GetShotsFired();
            var randomPoint = new PointOnBoard(1, 1);
            var boardLength = 5;

            //Act
            var pointCanBeShip = CanPointBeShip(randomPoint, shotsFired, boardLength);

            //Assert
            pointCanBeShip.Should().BeTrue();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(4, 1), 1),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(5, 1), 2),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(1, 3), 3),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(2, 3), 4),
                new Shot(ShotResult.HitAndSunk, board.Object, new PointOnBoard(3, 3), 5),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(4, 3), 6),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(5, 3), 7),
            };

            ShipOnBoard[] GetShipsOnBoard() => new[]
            {
                new ShipOnBoard(new Ship("1", 2), new PointOnBoard(4, 1), new PointOnBoard(5, 1)),
                new ShipOnBoard(new Ship("2", 5), new PointOnBoard(1, 3), new PointOnBoard(5, 3)),
                new ShipOnBoard(new Ship("3", 2), new PointOnBoard(1, 5), new PointOnBoard(2, 5)),
            };

            #endregion
        }

        [Test]
        public void PointInCornerFiveFreeFieldsToBorder_ShipFitsInSize_CanPointBeShipMethod_ReturnsTrue()
        {
            //Arrange
            var board = new Mock<GameBoardWithPlayer>();
            board.Setup(d => d.Board.GetOppositePlayer(It.IsAny<Player>()).Pattern.ShipsOnBoard).Returns(GetShipsOnBoard);

            var shotsFired = GetShotsFired();
            var randomPoint = new PointOnBoard(1, 1);
            var boardLength = 5;

            //Act
            var pointCanBeShip = CanPointBeShip(randomPoint, shotsFired, boardLength);

            //Assert
            pointCanBeShip.Should().BeTrue();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(3, 1), 1),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(3, 2), 2),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(3, 3), 3),
                new Shot(ShotResult.Hit, board.Object, new PointOnBoard(3, 4), 4),
                new Shot(ShotResult.HitAndSunk, board.Object, new PointOnBoard(3, 5), 5),
            };

            ShipOnBoard[] GetShipsOnBoard() => new[]
            {
                new ShipOnBoard(new Ship("1", 5), new PointOnBoard(3, 1), new PointOnBoard(3, 5)),
                new ShipOnBoard(new Ship("2", 5), new PointOnBoard(5, 1), new PointOnBoard(5, 5)),
            };
            #endregion
        }
    }
}
