namespace Bookify.Domain.Apartments;

public record Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string PostalCode { get; }

    public Address(string street, string city, string state, string country, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentException("Street cannot be null or empty.", nameof(street));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City cannot be null or empty.", nameof(city));
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            throw new ArgumentException("State cannot be null or empty.", nameof(state));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be null or empty.", nameof(country));
        }

        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new ArgumentException("PostalCode cannot be null or empty.", nameof(postalCode));
        }

        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }
}
