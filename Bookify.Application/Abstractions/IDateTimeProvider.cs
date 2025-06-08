using Bookify.Domain.Shared;

namespace Bookify.Application.Abstractions
{
    public interface IDateTimeProvider
    {
        UtcDateTime Now { get; }
    }
}
