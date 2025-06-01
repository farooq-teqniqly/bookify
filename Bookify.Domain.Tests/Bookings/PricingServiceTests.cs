using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using FluentAssertions;

namespace Bookify.Domain.Tests.Bookings
{
    public class PricingServiceTests
    {
        private static readonly Currency _defaultCurrency = Currency.Usd;

        [Fact]
        public void CalculatePrice_ShouldApplyAmenitiesUpCharge()
        {
            // Arrange
            var amenities = new List<Amenity> { Amenity.GardenView, Amenity.AirConditioning };
            var apartment = CreateApartment(amenities: amenities);
            var period = CreateDateRange(4);
            var service = new PricingService();

            // Act
            var result = service.CalculatePrice(apartment, period);

            // Assert
            var basePrice = 100m * 4; // 400
            var upCharge = basePrice * (0.05m + 0.01m); // 400 * 0.06 = 24
            result.PriceForPeriod.Amount.Should().Be(400m);
            result.AmenitiesUpCharge.Amount.Should().Be(upCharge);
            result.TotalPrice.Amount.Should().Be(424m);
        }

        [Fact]
        public void CalculatePrice_ShouldHandleNoAmenities()
        {
            // Arrange
            var apartment = CreateApartment(
                pricePerDay: 120m,
                cleaningFee: 0m,
                amenities: new List<Amenity>()
            );
            var period = CreateDateRange(1);
            var service = new PricingService();

            // Act
            var result = service.CalculatePrice(apartment, period);

            // Assert
            result.PriceForPeriod.Amount.Should().Be(120m);
            result.AmenitiesUpCharge.Amount.Should().Be(0m);
            result.ApartmentCleaningFee.Amount.Should().Be(0m);
            result.TotalPrice.Amount.Should().Be(120m);
        }

        [Fact]
        public void CalculatePrice_ShouldIncludeCleaningFee()
        {
            // Arrange
            var apartment = CreateApartment(cleaningFee: 50m);
            var period = CreateDateRange(2);
            var service = new PricingService();

            // Act
            var result = service.CalculatePrice(apartment, period);

            // Assert
            result.PriceForPeriod.Amount.Should().Be(200m);
            result.ApartmentCleaningFee.Amount.Should().Be(50m);
            result.AmenitiesUpCharge.Amount.Should().Be(0m);
            result.TotalPrice.Amount.Should().Be(250m);
        }

        [Fact]
        public void CalculatePrice_ShouldReturnBasePrice_WhenNoAmenitiesOrCleaningFee()
        {
            // Arrange
            var apartment = CreateApartment();
            var period = CreateDateRange(3);
            var service = new PricingService();

            // Act
            var result = service.CalculatePrice(apartment, period);

            // Assert
            result.PriceForPeriod.Amount.Should().Be(300m);
            result.AmenitiesUpCharge.Amount.Should().Be(0m);
            result.ApartmentCleaningFee.Amount.Should().Be(0m);
            result.TotalPrice.Amount.Should().Be(300m);
        }

        [Fact]
        public void CalculatePrice_ShouldSumAllComponents()
        {
            // Arrange
            var amenities = new List<Amenity> { Amenity.GardenView, Amenity.Parking };
            var apartment = CreateApartment(
                pricePerDay: 80m,
                cleaningFee: 20m,
                amenities: amenities
            );
            var period = CreateDateRange(5);
            var service = new PricingService();

            // Act
            var result = service.CalculatePrice(apartment, period);

            // Assert
            var basePrice = 80m * 5; // 400
            var upCharge = basePrice * (0.05m + 0.01m); // 400 * 0.06 = 24
            var total = basePrice + 20m + upCharge; // 400 + 20 + 24 = 444
            result.PriceForPeriod.Amount.Should().Be(400m);
            result.ApartmentCleaningFee.Amount.Should().Be(20m);
            result.AmenitiesUpCharge.Amount.Should().Be(24m);
            result.TotalPrice.Amount.Should().Be(444m);
        }

        [Fact]
        public void CalculatePrice_ShouldThrowArgumentNullException_WhenApartmentIsNull()
        {
            // Arrange
            var pricingService = new PricingService();
            DateRange period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 5));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => pricingService.CalculatePrice(null!, period)
            );
        }

        [Fact]
        public void CalculatePrice_ShouldThrowArgumentNullException_WhenPeriodIsNull()
        {
            // Arrange
            var pricingService = new PricingService();
            var apartment = CreateApartment();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => pricingService.CalculatePrice(apartment, null!)
            );
        }

        private static Apartment CreateApartment(
            decimal pricePerDay = 100m,
            decimal cleaningFee = 0m,
            List<Amenity>? amenities = null
        )
        {
            return new Apartment(
                Guid.NewGuid(),
                new Address("Street", "City", "State", "Zip", "Country"),
                new Money(cleaningFee, _defaultCurrency),
                new Description("desc"),
                new Name("Test Apartment"),
                new Money(pricePerDay, _defaultCurrency),
                amenities ?? new List<Amenity>()
            );
        }

        private static DateRange CreateDateRange(int days)
        {
            var start = DateOnly.FromDateTime(DateTime.Today);
            var end = start.AddDays(days);
            return DateRange.Create(start, end);
        }
    }
}
