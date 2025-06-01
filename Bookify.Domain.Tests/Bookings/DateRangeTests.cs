using Bookify.Domain.Bookings;
using FluentAssertions;

namespace Bookify.Domain.Tests.Bookings
{
    public class DateRangeTests
    {
        [Fact]
        public void Create_ShouldReturnDateRange_WhenStartIsBeforeEnd()
        {
            // Arrange
            var start = new DateOnly(2024, 6, 1);
            var end = new DateOnly(2024, 6, 5);

            // Act
            var range = DateRange.Create(start, end);

            // Assert
            range.Start.Should().Be(start);
            range.End.Should().Be(end);
            range.DurationInDays.Should().Be(4);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenStartAfterEnd()
        {
            // Arrange
            var start = new DateOnly(2024, 6, 5);
            var end = new DateOnly(2024, 6, 1);

            // Act
            Action act = () => DateRange.Create(start, end);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Start date must be before end date.");
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenStartEqualsEnd()
        {
            // Arrange
            var date = new DateOnly(2024, 6, 1);

            // Act
            Action act = () => DateRange.Create(date, date);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Start date must be before end date.");
        }

        [Fact]
        public void DurationInDays_ShouldReturnCorrectValue()
        {
            // Arrange
            var start = new DateOnly(2024, 1, 1);
            var end = new DateOnly(2024, 1, 10);
            var range = DateRange.Create(start, end);

            // Act
            var duration = range.DurationInDays;

            // Assert
            duration.Should().Be(9);
        }
    }
}
