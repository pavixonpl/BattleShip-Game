using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipGame.Core.Domain.Pattern;
using BattleshipGame.Core.Services.BoardPatternGenerator;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BattleshipGame.Tests.Unit.Core.Services.BoardPatternGenerator
{
    public class ClassicBoardGeneratorServiceTests
    {

        #region No Ships On Board Returns Correct Endpoint

        [Test]
        public void NoShipsOnBoard_ShipGoingLeft_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = new List<ShipOnBoard>();
            var expectedEndpoint = new PointOnBoard(2, 3);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingLeftEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        [Test]
        public void NoShipsOnBoard_ShipGoingRight_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = new List<ShipOnBoard>();
            var expectedEndpoint = new PointOnBoard(4, 3);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingRightEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        [Test]
        public void NoShipsOnBoard_ShipGoingUp_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = new List<ShipOnBoard>();
            var expectedEndpoint = new PointOnBoard(3, 2);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingUpEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        [Test]
        public void NoShipsOnBoard_ShipGoingDown_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = new List<ShipOnBoard>();
            var expectedEndpoint = new PointOnBoard(3, 4);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingDownEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        #endregion

        #region Other Ships On Board Not Disturbing Creation

        [Test]
        public void OtherShipsOnBoardNotDisturbingCreation_ShipGoingLeft_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(2, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();
            var expectedEndpoint = new PointOnBoard(1, 3);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingLeftEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(4,3), new PointOnBoard(4,5)),
            };

            #endregion
        }

        [Test]
        public void OtherShipsOnBoardNotDisturbingCreation_ShipGoingRight_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(1, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();
            var expectedEndpoint = new PointOnBoard(2, 3);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingRightEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(4,3), new PointOnBoard(4,5)),
            };

            #endregion
        }

        [Test]
        public void OtherShipsOnBoardNotDisturbingCreation_ShipGoingUp_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(2, 4);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();
            var expectedEndpoint = new PointOnBoard(2, 3);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingUpEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(4,3), new PointOnBoard(4,5)),
            };

            #endregion
        }

        [Test]
        public void OtherShipsOnBoardNotDisturbingCreation_ShipGoingDown_ReturnsCorrectEndpoint()
        {
            //Arrange
            var startPoint = new PointOnBoard(2, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();
            var expectedEndpoint = new PointOnBoard(2, 4);

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingDownEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeEquivalentTo(expectedEndpoint);

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(4,3), new PointOnBoard(4,5)),
            };

            #endregion
        }

        #endregion

        #region Other Ships On Board Disturbing Creation

        [Test]
        public void OtherShipsOnBoardDisturbingCreation_ShipGoingLeft_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingLeftEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeNull();

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(1,3), new PointOnBoard(1,5)),
            };

            #endregion
        }

        [Test]
        public void OtherShipsOnBoardDisturbingCreation_ShipGoingRight_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(2, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingRightEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeNull();

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(4,3), new PointOnBoard(4,5)),
            };

            #endregion
        }

        [Test]
        public void OtherShipsOnBoardDisturbingCreation_ShipGoingUp_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingUpEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeNull();

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(1,3), new PointOnBoard(1,5)),
            };

            #endregion
        }

        [Test]
        public void OtherShipsOnBoardDisturbingCreation_ShipGoingDown_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(3, 3);
            var shipToCreate = GetShip();
            var alreadyExistingShips = GetCurrentShips();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingDownEndpoint.Invoke(startPoint, shipToCreate, alreadyExistingShips);

            //Assert
            actualEndpoint.Should().BeNull();

            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            ShipOnBoard[] GetCurrentShips() => new[]
            {
                new ShipOnBoard(new Ship("1", 3), new PointOnBoard(1,1), new PointOnBoard(3,1)),
                new ShipOnBoard(new Ship("2", 3), new PointOnBoard(1,3), new PointOnBoard(1,5)),
                new ShipOnBoard(new Ship("3", 3), new PointOnBoard(3,5), new PointOnBoard(5,5)),
            };

            #endregion
        }

        #endregion

        #region Ship Will Go Over The Border

        [Test]
        public void ShipWillGoOverTheBorder_ShipGoingLeft_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(1, 3);
            var shipToCreate = GetShip();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingLeftEndpoint.Invoke(startPoint, shipToCreate, new List<ShipOnBoard>());

            //Assert
            actualEndpoint.Should().BeNull();


            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        [Test]
        public void ShipWillGoOverTheBorder_ShipGoingRight_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(10, 1);
            var shipToCreate = GetShip();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingRightEndpoint.Invoke(startPoint, shipToCreate, new List<ShipOnBoard>());

            //Assert
            actualEndpoint.Should().BeNull();


            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        [Test]
        public void ShipWillGoOverTheBorder_ShipGoingUp_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(5, 1);
            var shipToCreate = GetShip();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingUpEndpoint.Invoke(startPoint, shipToCreate, new List<ShipOnBoard>());

            //Assert
            actualEndpoint.Should().BeNull();


            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        [Test]
        public void ShipWillGoOverTheBorder_ShipGoingDown_ReturnsNull()
        {
            //Arrange
            var startPoint = new PointOnBoard(5, 10);
            var shipToCreate = GetShip();

            //Act
            var actualEndpoint = new ClassicBoardGeneratorService().GetShipGoingDownEndpoint.Invoke(startPoint, shipToCreate, new List<ShipOnBoard>());

            //Assert
            actualEndpoint.Should().BeNull();


            #region Local Methods

            Ship GetShip() => new Ship("newShip", 2);

            #endregion
        }

        #endregion
    }
}
