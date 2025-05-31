using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using FluentAssertions;

namespace Bookify.Domain.Tests.Bookings
{
    public class PricingDetailsTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var priceForPeriod = new Money(100m, Currency.Usd);
            var cleaningFee = new Money(20m, Currency.Usd);
            var amenitiesUpCharge = new Money(10m, Currency.Usd);
            var totalPrice = new Money(130m, Currency.Usd);

            // Act
            var details = new PricingDetails(
                priceForPeriod,
                cleaningFee,
                amenitiesUpCharge,
                totalPrice
            );

            // Assert
            details.PriceForPeriod.Should().Be(priceForPeriod);
            details.ApartmentCleaningFee.Should().Be(cleaningFee);
            details.AmenitiesUpCharge.Should().Be(amenitiesUpCharge);
            details.TotalPrice.Should().Be(totalPrice);
        }

        [Theory]
        [InlineData(null, "valid", "valid", "valid")]
        [InlineData("valid", null, "valid", "valid")]
        [InlineData("valid", "valid", null, "valid")]
        [InlineData("valid", "valid", "valid", null)]
        public void Constructor_ShouldThrowArgumentNullException_WhenAnyArgumentIsNull(
            string? priceForPeriodFlag,
            string? cleaningFeeFlag,
            string? amenitiesUpChargeFlag,
            string? totalPriceFlag
        )
        {
            // Arrange
            Money priceForPeriod =
                priceForPeriodFlag == "valid" ? new Money(1, Currency.Usd) : null!;

            Money cleaningFee = cleaningFeeFlag == "valid" ? new Money(2, Currency.Usd) : null!;

            Money amenitiesUpCharge =
                amenitiesUpChargeFlag == "valid" ? new Money(3, Currency.Usd) : null!;

            Money totalPrice = totalPriceFlag == "valid" ? new Money(4, Currency.Usd) : null!;

            // Act
            Action act = () =>
                _ = new PricingDetails(priceForPeriod, cleaningFee, amenitiesUpCharge, totalPrice);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
