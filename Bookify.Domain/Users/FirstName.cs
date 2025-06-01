namespace Bookify.Domain.Users;

public sealed record FirstName
{
    public string Value { get; }

    public FirstName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}
