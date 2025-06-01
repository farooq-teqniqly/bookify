using Bookify.Domain.Users;
using FluentAssertions;

namespace Bookify.Domain.Tests.Users
{
    public class LastNameTests
    {
        [Fact]
        public void Constructor_ShouldSetValue_WhenValidStringProvided()
        {
            // Arrange
            var validLastName = "Smith";

            // Act
            var lastName = new LastName(validLastName);

            // Assert
            lastName.Value.Should().Be(validLastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsNullOrWhiteSpace(
            string? invalidValue
        )
        {
            // Act
            Action act = () => _ = new LastName(invalidValue!);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
