using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent : IDomainEvent
{
    public Guid BookingId { get; }

    public BookingConfirmedDomainEvent(Guid bookingId)
    {
        BookingId = bookingId;
    }
}
