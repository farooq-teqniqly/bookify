using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent : IDomainEvent
{
    public Guid BookingId { get; }

    public BookingCompletedDomainEvent(Guid bookingId)
    {
        BookingId = bookingId;
    }
}
