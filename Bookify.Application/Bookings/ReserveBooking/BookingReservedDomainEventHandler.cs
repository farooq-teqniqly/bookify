using Bookify.Application.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Users;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking
{
    internal sealed class BookingReservedDomainEventHandler
        : INotificationHandler<BookingReservedDomainEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public BookingReservedDomainEventHandler(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IEmailService emailService
        )
        {
            _bookingRepository =
                bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));

            _userRepository =
                userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task Handle(
            BookingReservedDomainEvent notification,
            CancellationToken cancellationToken
        )
        {
            var getBookingResult = await _bookingRepository.GetByIdAsync(
                notification.BookingId,
                cancellationToken
            );

            if (getBookingResult.IsFailure)
            {
                return;
            }

            var booking = getBookingResult.GetValue();
            var getUserResult = await _userRepository.GetByIdAsync(
                booking.UserId,
                cancellationToken
            );

            if (getUserResult.IsFailure)
            {
                return;
            }

            var user = getUserResult.GetValue();

            await _emailService.SendAsync(
                user.Email,
                "Booking reserved",
                "You have 10 minutes to confirm this booking."
            );
        }
    }
}
