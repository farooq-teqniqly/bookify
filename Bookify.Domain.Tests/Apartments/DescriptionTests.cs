using Bookify.Domain.Apartments;
using FluentAssertions;

namespace Bookify.Domain.Tests.Apartments
{
    public class DescriptionTests
    {
        [Fact]
        public void Constructor_ShouldSetValue_WhenValidStringProvided()
        {
            // Arrange
            var validValue = "A nice apartment.";

            // Act
            var description = new Description(validValue);

            // Assert
            description.Value.Should().Be(validValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsNullOrWhitespace(
            string? invalidValue
        )
        {
            // Act
            Action act = () => _ = new Description(invalidValue!); // Assign to discard variable to avoid CA1806

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Description cannot be null or whitespace.*")
                .And.ParamName.Should()
                .Be("value");
        }
    }
}
