using Bookify.Domain.Apartments;
using Bookify.Results;

namespace Bookify.Domain.Bookings
{
    public interface IBookingRepository
    {
        Task<Result<Booking>> AddBookingAsync(
            Booking booking,
            CancellationToken cancellationToken = default
        );
        Task<Result<Booking>> GetByIdAsync(
            Guid notificationBookingId,
            CancellationToken cancellationToken = default
        );
        Task<Result<Booking>> IsOverlappingAsync(
            Apartment apartment,
            DateRange dateRange,
            CancellationToken cancellationToken = default
        );
    }
}
