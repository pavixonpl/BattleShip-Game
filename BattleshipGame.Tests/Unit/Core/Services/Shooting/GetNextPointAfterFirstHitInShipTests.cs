using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    public class GetNextPointAfterFirstHitInShipTests
    {
        private GameBoardWithPlayer _gameBoard;

        [SetUp]
        public void SetUp()
        {
            var board = new Mock<GameBoardWithPlayer>();
            board.Setup(d => d.Pattern.LengthAndHeightOfBoard).Returns(10);
            _gameBoard = board.Object;
        }

        [Test]
        public void AllNearbyPointsAreMiss_ReturnsNull()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeNull();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 1),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 4), 2),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 2), 3),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 3), 4),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(4, 3), 5),
            };

            #endregion
        }

        [Test]
        public void LastHitWasToDestroyedShip_ReturnsNull()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeNull();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 1),
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 4), 2),
            };

            #endregion
        }

        /// <summary>
        ///Ship cannot be created like this, but I'm checking expected behaviour, that if nothing is to shoot because of ships nearby, then it returns null
        /// </summary>
        [Test]
        public void AllNearbyPointsAreClaimedByOtherShips_ReturnsNull()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeNull();

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 1), 1),
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(5, 3), 2),
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 5), 3),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 3), 4),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 5),
            };

            #endregion
        }

        [Test]
        public void AllExceptOnePointAreClaimedByOtherNotDestroyedShips_ReturnsHim()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            var expectedPoint = new PointOnBoard(3, 4);

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeEquivalentTo(expectedPoint);

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 1),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(5, 2), 2),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(5, 5), 3),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 4), 4),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 5),
            };

            #endregion
        }

        [Test]
        public void AllExceptOnePointAreClaimedByOtherDestroyedShips_ReturnsHim()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            var expectedPoint = new PointOnBoard(3, 4);

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeEquivalentTo(expectedPoint);

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(2, 1), 1),
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(5, 2), 2),
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(5, 5), 3),
                new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(1, 4), 4),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 5),
            };

            #endregion
        }

        [Test]
        public void AllExceptOnePointsAreMissedShots_HitBeforeMisses_ReturnsHim()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            var expectedPoint = new PointOnBoard(4, 3);

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeEquivalentTo(expectedPoint);

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 1),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 4), 2),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 2), 3),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 3), 4),
            };

            #endregion
        }

        [Test]
        public void AllExceptOnePointsAreMissedShots_HitAfterMisses_ReturnsHim()
        {
            //Arrange
            var shotsFired = GetShotsFired();

            var expectedPoint = new PointOnBoard(4, 3);

            //Act
            var actualPoint = new ShootingService().GetNextPointAfterFirstHitInShip(shotsFired);

            //Assert
            actualPoint.Should().BeEquivalentTo(expectedPoint);

            #region Local Methods

            Shot[] GetShotsFired() => new[]
            {
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 4), 1),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 2), 2),
                new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 3), 3),
                new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 4),
            };

            #endregion
        }
    }
}
