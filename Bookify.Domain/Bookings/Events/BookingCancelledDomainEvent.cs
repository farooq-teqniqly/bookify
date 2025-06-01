using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCancelledDomainEvent : IDomainEvent
{
    public Guid BookingId { get; }

    public BookingCancelledDomainEvent(Guid bookingId)
    {
        BookingId = bookingId;
    }
}
