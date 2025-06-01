namespace Bookify.Domain.Users;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        Value = value;
    }
}
