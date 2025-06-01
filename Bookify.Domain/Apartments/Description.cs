namespace Bookify.Domain.Apartments;

public sealed record Description
{
    public string Value { get; }

    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(value));
        }

        Value = value;
    }
}
