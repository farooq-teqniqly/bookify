using Bookify.Results;

namespace Bookify.Domain.Bookings;

public sealed class BookingError : Error
{
    public BookingError(string code, string message, Guid bookingId)
        : base(code, message, new Dictionary<string, object> { { "bookingId", bookingId } })
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; }

    public static BookingError AlreadyStarted(Guid bookingId) =>
        new("Booking.AlreadyStarted", "The booking has already started.", bookingId);

    public static BookingError NotConfirmed(Guid bookingId) =>
        new("Booking.NotConfirmed", "The booking has not been confirmed.", bookingId);

    public static BookingError NotReserved(Guid bookingId) =>
        new("Booking.NotReserved", "The booking has not been reserved.", bookingId);

    public static BookingError Overlap(Guid overlappingBookingId) =>
        new(
            "Booking.Overlap",
            "This booking overlaps with an existing booking.",
            overlappingBookingId
        );
}
