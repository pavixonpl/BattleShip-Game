using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipGame.Core.Domain.Pattern;
using FluentAssertions;
using NUnit.Framework;

namespace BattleshipGame.Tests.Unit.Core.Domain.Pattern
{
    public class PointOnBoardTests
    {
        [Test]
        public void GetPointsClaimedByThisPoint_ShouldReturnAll9Points()
        {
            //Arrange
            var point = new PointOnBoard(3, 3);
            var expectedPoints = GetExpectedPoints();

            //Act
            var actualPointsClaimed = point.GetPointsClaimedByThisPoint();

            //Assert
            actualPointsClaimed.Should().BeEquivalentTo(expectedPoints);

            #region MyRegion

            PointOnBoard[] GetExpectedPoints() => new[]
            {
                new PointOnBoard(3,3),

                new PointOnBoard(3,2),
                new PointOnBoard(3,4),
                new PointOnBoard(2,3),
                new PointOnBoard(4,3),

                new PointOnBoard(2,2),
                new PointOnBoard(4,4),
                new PointOnBoard(2,4),
                new PointOnBoard(4,2),
            };

            #endregion
        }
    }
}
