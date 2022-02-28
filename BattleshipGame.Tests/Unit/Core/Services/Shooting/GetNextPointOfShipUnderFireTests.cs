using BattleshipGame.Core.Domain.Enums;
using BattleshipGame.Core.Domain.Gameplay;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.ShotProvider;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BattleshipGame.Tests.Unit.Core.Services.Shooting;

public class GetNextPointOfShipUnderFireTests
{
    private GameBoardWithPlayer _gameBoard;

    [SetUp]
    public void SetUp()
    {
        var board = new Mock<GameBoardWithPlayer>();
        board.Setup(d => d.Pattern.LengthAndHeightOfBoard).Returns(10);
        _gameBoard = board.Object;
    }

    #region Last Hit Was To Already Sunk Ship

    [Test]
    public void LastHitWasToAlreadySunkShip_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 4), 2),
            new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 5), 3),
        };

        #endregion
    }

    #endregion

    #region Only One Hit To Ship

    [Test]
    public void OnlyOneHitToShip_FirstHitInGame_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 1),
        };

        #endregion
    }

    [Test]
    public void OnlyOneHitToShip_BeforeHitOneShipWasDestroyed_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 3), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 4), 2),
            new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 5), 3),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 1), 4),
        };

        #endregion
    }

    #endregion

    #region Point On End Is Claimed By Destroyed Ship

    [Test]
    public void PointOnEndIsClaimedByDestroyedShip_OnLeftOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(5, 2);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 2), 2),
            new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(1, 3), 3),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(4, 2), 4),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 2), 5),
        };

        #endregion
    }

    [Test]
    public void PointOnEndIsClaimedByDestroyedShip_OnRightOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(1, 2);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(5, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(5, 2), 2),
            new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(5, 3), 3),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 4),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 2), 5),
        };

        #endregion
    }

    [Test]
    public void PointOnEndIsClaimedByDestroyedShip_OnUpOfYAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 5);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 2),
            new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 1), 3),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 4), 4),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 5),
        };

        #endregion
    }

    [Test]
    public void PointOnEndIsClaimedByDestroyedShip_OnDownOfYAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 5), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 5), 2),
            new Shot(ShotResult.HitAndSunk, _gameBoard, new PointOnBoard(3, 5), 3),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 4),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 5),
        };

        #endregion
    }

    #endregion

    #region Point On End Is Shot With Miss

    [Test]
    public void PointOnEndIsShotWithMiss_OnLeftOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(4, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(1, 1), 3),
        };

        #endregion
    }

    [Test]
    public void PointOnEndIsShotWithMiss_OnRightOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(1, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(4, 1), 3),
        };

        #endregion
    }

    [Test]
    public void PointOnEndIsShotWithMiss_OnUpOfYAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 4);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 1), 3),
        };

        #endregion
    }

    [Test]
    public void PointOnEndIsShotWithMiss_OnDownOfYAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 4), 3),
        };

        #endregion
    }

    #endregion

    #region Miss On End Was Before Hits

    [Test]
    public void MissOnEndWasBeforeHits_OnLeftOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(4, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Miss,_gameBoard, new PointOnBoard(1, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 1), 2),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 3),
        };

        #endregion
    }

    [Test]
    public void MissOnEndWasBeforeHits_OnRightOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(1, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(4, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 2),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 1), 3),
        };

        #endregion
    }

    [Test]
    public void MissOnEndWasBeforeHits_OnUpOYXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2,4);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 2),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 3),
        };

        #endregion
    }

    [Test]
    public void MissOnEndWasBeforeHits_OnDownOfYAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 4), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 2),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 3),
        };

        #endregion
    }

    #endregion

    #region Miss On End Was After Hits

    [Test]
    public void MissOnEndWasAfterHits_OnLeftOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(4, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(1, 1), 3),
        };

        #endregion
    }

    [Test]
    public void MissOnEndWasAfterHits_OnRightOfXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(1, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(3, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(4, 1), 3),
        };

        #endregion
    }

    [Test]
    public void MissOnEndWasAfterHits_OnUpOYXAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 4);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 1), 3),
        };

        #endregion
    }

    [Test]
    public void MissOnEndWasAfterHits_OnDownOfYAxis_ReturnsSecondEnd()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        var expectedPoint = new PointOnBoard(2, 1);

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeEquivalentTo(expectedPoint);

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 2), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 3), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(2, 4), 3),
        };

        #endregion
    }

    #endregion

    #region Does Not Create Shots Outside Border

    /// <summary>
    /// In normal case it will shoot to 3,1 but now it's blocked to force application to use 0,1 point
    /// </summary>
    [Test]
    public void DoesNotCreateShotsOutsideBorder_OnLeftOfXAxis_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(2, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(3, 1), 3),
        };

        #endregion
    }

    /// <summary>
    /// In normal case it will shoot to 8,1 but now it's blocked to force application to use 11,1 point
    /// </summary>
    [Test]
    public void DoesNotCreateShotsOutsideBorder_OnRightOfXAxis_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(9, 1), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(10, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(8, 1), 3),
        };

        #endregion
    }

    /// <summary>
    /// In normal case it will shoot to 1,3 but now it's blocked to force application to use 1,0 point
    /// </summary>
    [Test]
    public void DoesNotCreateShotsOutsideBorder_OnUpOfYAxis_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 2), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 1), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(1, 3), 3),
        };

        #endregion
    }

    /// <summary>
    /// In normal case it will shoot to 1,8 but now it's blocked to force application to use 1,11 point
    /// </summary>
    [Test]
    public void DoesNotCreateShotsOutsideBorder_OnDownOfYAxis_ReturnsNull()
    {
        //Arrange
        var shotsFired = GetShotsFired();

        //Act
        var actualPoint = new ShootingService().GetNextPointOfShipUnderFire(shotsFired);

        //Assert
        actualPoint.Should().BeNull();

        #region Local Methods

        Shot[] GetShotsFired() => new[]
        {
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 9), 1),
            new Shot(ShotResult.Hit, _gameBoard, new PointOnBoard(1, 10), 2),
            new Shot(ShotResult.Miss, _gameBoard, new PointOnBoard(1, 8), 3),
        };

        #endregion
    }

    #endregion
}