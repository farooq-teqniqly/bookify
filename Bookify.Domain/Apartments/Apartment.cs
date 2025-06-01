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

        public Address Address { get; }
        public List<Amenity> Amenities { get; }
        public Money CleaningFee { get; internal set; }
        public Description Description { get; }
        public UtcDateTime? LastBookedOn { get; internal set; }
        public Name Name { get; }
        public Money Price { get; internal set; }
    }
}
