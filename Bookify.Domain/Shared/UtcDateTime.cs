namespace Bookify.Domain.Shared
{
    public sealed record UtcDateTime
    {
        public static readonly UtcDateTime Now = new(DateTimeOffset.UtcNow);

        private readonly DateTimeOffset _value;

        private UtcDateTime(DateTimeOffset value) => _value = value;

        public DateTimeOffset Value => _value;

        public DateOnly GetDate() => DateOnly.FromDateTime(_value.UtcDateTime);
    }
}
