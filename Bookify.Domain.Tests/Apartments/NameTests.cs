using Bookify.Domain.Apartments;
using FluentAssertions;

namespace Bookify.Domain.Tests.Apartments
{
    public class NameTests
    {
        [Fact]
        public void Constructor_Should_Set_Value_When_Valid()
        {
            // Arrange
            var validName = "Apartment 101";

            // Act
            var name = new Name(validName);

            // Assert
            name.Value.Should().Be(validName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_ArgumentException_When_Value_Is_Null_Or_Whitespace(
            string? invalidValue
        )
        {
            // Act
            Action act = () => _ = new Name(invalidValue!);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Name cannot be null or whitespace.*")
                .And.ParamName.Should()
                .Be("value");
        }

        [Fact]
        public void Name_Records_With_Different_Values_Should_Not_Be_Equal()
        {
            // Arrange
            var name1 = new Name("Name1");
            var name2 = new Name("Name2");

            // Assert
            name1.Should().NotBe(name2);
        }

        [Fact]
        public void Name_Records_With_Same_Value_Should_Be_Equal()
        {
            // Arrange
            var name1 = new Name("TestName");
            var name2 = new Name("TestName");

            // Assert
            name1.Should().Be(name2);
        }
    }
}
