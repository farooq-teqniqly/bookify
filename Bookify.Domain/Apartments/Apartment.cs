using Bookify.Domain.Abstractions;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments
{
    public sealed class Apartment : Entity
    {
        public Apartment(
            Guid id,
            Address address,
            Money cleaningFee,
            Description description,
            Name name,
            Money price,
            List<Amenity> amenities
        )
            : base(id)
        {
            ArgumentNullException.ThrowIfNull(address);
            ArgumentNullException.ThrowIfNull(cleaningFee);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(price);
            ArgumentNullException.ThrowIfNull(amenities);

            Address = address;
            CleaningFee = cleaningFee;
            Description = description;
            Name = name;
            Price = price;
            Amenities = amenities;
        }

        public Address Address { get; private set; }
        public List<Amenity> Amenities { get; private set; }
        public Money CleaningFee { get; set; }
        public Description Description { get; private set; }
        public UtcDateTime? LastBookedOn { get; internal set; }
        public Name Name { get; private set; }
        public Money Price { get; set; }
    }
}
