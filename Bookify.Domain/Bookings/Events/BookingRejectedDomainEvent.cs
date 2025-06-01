using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent : IDomainEvent
{
    public Guid BookingId { get; }

    public BookingRejectedDomainEvent(Guid bookingId)
    {
        BookingId = bookingId;
    }
}
