namespace Bookify.Domain.Users;

public sealed record LastName
{
    public string Value { get; }

    public LastName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}
