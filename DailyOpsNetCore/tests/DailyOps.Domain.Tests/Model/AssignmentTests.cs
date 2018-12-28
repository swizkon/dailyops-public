using System;
using System.Collections.Generic;
using Xunit;

namespace DailyOps.Domain.Model.Tests
{
    public class AssignmentTests
    {

        [Theory]
        [MemberData(nameof(Data))]
        public void WhenMarkedAsCompleted_ThenItSetsDateIntervals(
            Reccurence reccurence, 
            DateTime completionDate, 
            DateTime expectedReapperance)
        {
            // Arrange
            var sut = new Assignment(string.Empty, reccurence);

            // Act
            sut.MarkAsCompleted(completionDate);

            // Assert
            Assert.Equal(expectedReapperance.ToString("O"), sut.DateInterval.Reapperance.ToString("O"));
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { Reccurence.Daily, DateTime.Parse("2018-12-01 12:00:00"), DateTime.Parse("2018-12-02")},
            new object[] { Reccurence.Weekly, DateTime.Parse("2018-12-13 12:34:00"), DateTime.Parse("2018-12-17")},
            new object[] { Reccurence.Monthly, DateTime.Parse("2018-10-13 12:34:00"), DateTime.Parse("2018-11-01")},
            new object[] { Reccurence.Annual, DateTime.Parse("2018-10-13 12:34:00"), DateTime.Parse("2019-01-01")},
            new object[] { Reccurence.Annual, DateTime.Parse("2019-02-13 12:34:00"), DateTime.Parse("2019-04-01")}
        };
    }
}
