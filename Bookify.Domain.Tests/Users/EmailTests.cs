using Bookify.Domain.Users;
using FluentAssertions;

namespace Bookify.Domain.Tests.Users
{
    public class EmailTests
    {
        [Fact]
        public void Constructor_ShouldSetValue_WhenValidEmailIsProvided()
        {
            // Arrange
            var emailValue = "test@example.com";

            // Act
            var email = new Email(emailValue);

            // Assert
            email.Value.Should().Be(emailValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsNullOrEmpty(string? value)
        {
            // Act
            Action act = () => _ = new Email(value!);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
