using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events
{
    public sealed record BookingReservedDomainEvent : IDomainEvent
    {
        public Guid BookingId { get; }

        public BookingReservedDomainEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
