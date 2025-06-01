using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using FluentAssertions;

namespace Bookify.Domain.Tests.Apartments
{
    public class ApartmentTests
    {
        [Fact]
        public void CanSetCleaningFeeAndPrice()
        {
            var apartment = CreateApartment();
            var newCleaningFee = new Money(100, Currency.Usd);
            var newPrice = new Money(500, Currency.Usd);

            apartment.CleaningFee = newCleaningFee;
            apartment.Price = newPrice;

            apartment.CleaningFee.Should().Be(newCleaningFee);
            apartment.Price.Should().Be(newPrice);
        }

        [Fact]
        public void CanSetLastBookedOn()
        {
            var apartment = CreateApartment();
            var now = UtcDateTime.Now;

            // Use reflection to set the private/protected setter for testing purposes
            typeof(Apartment)
                .GetProperty(nameof(Apartment.LastBookedOn))!
                .SetValue(apartment, now);

            apartment.LastBookedOn.Should().Be(now);
        }

        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var id = Guid.NewGuid();
            var address = new Address("456 Elm St", "Town", "Region", "67890", "Country");
            var cleaningFee = new Money(75, Currency.Eur);
            var description = new Description("Spacious and modern");
            var name = new Name("Modern Loft");
            var price = new Money(300, Currency.Eur);
            var amenities = new List<Amenity> { Amenity.Parking, Amenity.Terrace };

            var apartment = new Apartment(
                id,
                address,
                cleaningFee,
                description,
                name,
                price,
                amenities
            );

            apartment.Id.Should().Be(id);
            apartment.Address.Should().Be(address);
            apartment.CleaningFee.Should().Be(cleaningFee);
            apartment.Description.Should().Be(description);
            apartment.Name.Should().Be(name);
            apartment.Price.Should().Be(price);
            apartment.Amenities.Should().BeEquivalentTo(amenities);
            apartment.LastBookedOn.Should().BeNull();
        }

        [Fact]
        public void Constructor_ThrowsIfArgumentNull()
        {
            var id = Guid.NewGuid();
            var address = new Address("789 Oak St", "Village", "Province", "54321", "Country");
            var cleaningFee = new Money(25, Currency.Cad);
            var description = new Description("Charming spot");
            var name = new Name("Charming Home");
            var price = new Money(150, Currency.Cad);
            var amenities = new List<Amenity> { Amenity.Spa };

            Action act1 = () =>
                new Apartment(id, null, cleaningFee, description, name, price, amenities);
            Action act2 = () =>
                new Apartment(id, address, null, description, name, price, amenities);
            Action act3 = () =>
                new Apartment(id, address, cleaningFee, null, name, price, amenities);
            Action act4 = () =>
                new Apartment(id, address, cleaningFee, description, null, price, amenities);
            Action act5 = () =>
                new Apartment(id, address, cleaningFee, description, name, null, amenities);
            Action act6 = () =>
                new Apartment(id, address, cleaningFee, description, name, price, null);

            act1.Should().Throw<ArgumentNullException>();
            act2.Should().Throw<ArgumentNullException>();
            act3.Should().Throw<ArgumentNullException>();
            act4.Should().Throw<ArgumentNullException>();
            act5.Should().Throw<ArgumentNullException>();
            act6.Should().Throw<ArgumentNullException>();
        }

        private static Apartment CreateApartment()
        {
            return new Apartment(
                Guid.NewGuid(),
                new Address("123 Main St", "City", "State", "12345", "Country"),
                new Money(50, Currency.Usd),
                new Description("Nice apartment"),
                new Name("Cozy Place"),
                new Money(200, Currency.Usd),
                new List<Amenity> { Amenity.WiFi, Amenity.Gym }
            );
        }
    }
}
