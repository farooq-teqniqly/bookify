namespace Bookify.Domain.Shared
{
    public record UtcDateTime
    {
        public static readonly UtcDateTime Now = new(DateTimeOffset.UtcNow);

        private UtcDateTime(DateTimeOffset value) => Value = value;

        public DateTimeOffset Value { get; init; }

        public DateOnly GetDate() => DateOnly.FromDateTime(Value.UtcDateTime);
    }
}
