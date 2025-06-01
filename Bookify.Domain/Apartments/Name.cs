﻿namespace Bookify.Domain.Apartments;

public record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(value));
        }

        Value = value;
    }
}
