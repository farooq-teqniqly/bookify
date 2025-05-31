using Bookify.Domain.Apartments;
using FluentAssertions;

namespace Bookify.Domain.Tests.Apartments
{
    public class AddressTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties_When_Valid_Arguments()
        {
            // Arrange
            var street = "123 Main St";
            var city = "Springfield";
            var state = "IL";
            var country = "USA";
            var postalCode = "62704";

            // Act
            var address = new Address(street, city, state, country, postalCode);

            // Assert
            address.Street.Should().Be(street);
            address.City.Should().Be(city);
            address.State.Should().Be(state);
            address.Country.Should().Be(country);
            address.PostalCode.Should().Be(postalCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_City_Is_Null_Or_Whitespace(string? invalidCity)
        {
            // Act
            Action act = () =>
            {
                _ = new Address("Street", invalidCity!, "State", "Country", "12345");
            };

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("City cannot be null or empty.*")
                .And.ParamName.Should()
                .Be("city");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Country_Is_Null_Or_Whitespace(
            string? invalidCountry
        )
        {
            // Act
            Action act = () =>
            {
                _ = new Address("Street", "City", "State", invalidCountry!, "12345");
            };

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Country cannot be null or empty.*")
                .And.ParamName.Should()
                .Be("country");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_PostalCode_Is_Null_Or_Whitespace(
            string? invalidPostalCode
        )
        {
            // Act
            Action act = () =>
            {
                _ = new Address("Street", "City", "State", "Country", invalidPostalCode!);
            };

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("PostalCode cannot be null or empty.*")
                .And.ParamName.Should()
                .Be("postalCode");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_State_Is_Null_Or_Whitespace(string? invalidState)
        {
            // Act
            Action act = () =>
            {
                _ = new Address("Street", "City", invalidState!, "Country", "12345");
            };

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("State cannot be null or empty.*")
                .And.ParamName.Should()
                .Be("state");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Street_Is_Null_Or_Whitespace(
            string? invalidStreet
        )
        {
            // Act
            Action act = () =>
            {
                _ = new Address(invalidStreet!, "City", "State", "Country", "12345");
            };

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Street cannot be null or empty.*")
                .And.ParamName.Should()
                .Be("street");
        }
    }
}
