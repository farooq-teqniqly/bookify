using Bookify.Domain.Users;
using FluentAssertions;

namespace Bookify.Domain.Tests.Users
{
    public class FirstNameTests
    {
        [Fact]
        public void Constructor_Should_Set_Value_When_Valid()
        {
            // Arrange
            var name = "John";

            // Act
            var firstName = new FirstName(name);

            // Assert
            firstName.Value.Should().Be(name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_ArgumentException_When_Value_Is_Null_Or_WhiteSpace(
            string? invalidValue
        )
        {
            // Act
            Action act = () => _ = new FirstName(invalidValue!);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
