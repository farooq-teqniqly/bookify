using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;
using Bookify.Results;
using FluentAssertions;
using NSubstitute;

namespace Bookify.Domain.Tests.Bookings
{
    public class BookingTests
    {
        [Fact]
        public void Cancel_ShouldFail_WhenBookingHasAlreadyStarted()
        {
            // Arrange
            var booking = CreatePastReservedBooking();
            booking.Confirm(UtcDateTime.Now);

            // Act
            var result = booking.Cancel(UtcDateTime.Now);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Cancel_ShouldFail_WhenStatusIsNotConfirmed()
        {
            // Arrange
            var booking = CreateReservedBooking();

            // Act
            var result = booking.Cancel(UtcDateTime.Now);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Cancel_ShouldRaise_BookingCancelledDomainEvent()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.Confirm(UtcDateTime.Now);
            booking.ClearDomainEvents();
            var cancelTime = UtcDateTime.Now;

            // Act
            var result = booking.Cancel(cancelTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = booking.GetDomainEvents();
            events.Should().ContainSingle(e => e is BookingCancelledDomainEvent);
        }

        [Fact]
        public void Cancel_ShouldSetStatusToCancelled_WhenStatusIsConfirmed_AndNotStarted()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.Confirm(UtcDateTime.Now);
            var cancelTime = UtcDateTime.Now;

            // Act
            var result = booking.Cancel(cancelTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            booking.Status.Should().Be(BookingStatus.Cancelled);
            booking.CancelledOn.Should().Be(cancelTime);
        }

        [Fact]
        public void Cancel_ShouldThrow_WhenCancelTimeIsNull()
        {
            // Arrange
            var booking = Booking.Reserve(
                CreateApartment(),
                CreateUser(),
                CreateDateRange(),
                UtcDateTime.Now,
                CreateDummyPricingService()
            );
            booking.Confirm(UtcDateTime.Now);

            // Act
            Action act = () => booking.Cancel(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Complete_ShouldFail_WhenStatusIsNotReserved()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.Confirm(UtcDateTime.Now);

            // Act
            var result = booking.Complete(UtcDateTime.Now);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Complete_ShouldRaise_BookingCompletedDomainEvent()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.ClearDomainEvents();
            var completionTime = UtcDateTime.Now;

            // Act
            var result = booking.Complete(completionTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = booking.GetDomainEvents();
            events.Should().ContainSingle(e => e is BookingCompletedDomainEvent);
        }

        [Fact]
        public void Complete_ShouldSetStatusToCompleted_WhenStatusIsReserved()
        {
            // Arrange
            var booking = CreateReservedBooking();
            var completionTime = UtcDateTime.Now;

            // Act
            var result = booking.Complete(completionTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            booking.Status.Should().Be(BookingStatus.Completed);
            booking.CompletedOn.Should().Be(completionTime);
        }

        [Fact]
        public void Complete_ShouldThrow_WhenCompletionTimeIsNull()
        {
            // Arrange
            var booking = Booking.Reserve(
                CreateApartment(),
                CreateUser(),
                CreateDateRange(),
                UtcDateTime.Now,
                CreateDummyPricingService()
            );

            // Act
            Action act = () => booking.Complete(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Confirm_ShouldFail_WhenStatusIsNotReserved()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.Confirm(UtcDateTime.Now);

            // Act
            var result = booking.Confirm(UtcDateTime.Now);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Confirm_ShouldRaise_BookingConfirmedDomainEvent()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.ClearDomainEvents();
            var confirmationTime = UtcDateTime.Now;

            // Act
            var result = booking.Confirm(confirmationTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = booking.GetDomainEvents();
            events.Should().ContainSingle(e => e is BookingConfirmedDomainEvent);
        }

        [Fact]
        public void Confirm_ShouldSetStatusToConfirmed_WhenStatusIsReserved()
        {
            // Arrange
            var booking = CreateReservedBooking();
            var confirmationTime = UtcDateTime.Now;

            // Act
            var result = booking.Confirm(confirmationTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            booking.Status.Should().Be(BookingStatus.Confirmed);
            booking.ConfirmedOn.Should().Be(confirmationTime);
        }

        [Fact]
        public void Confirm_ShouldThrow_WhenConfirmationTimeIsNull()
        {
            // Arrange
            var pricingService = Substitute.For<IPricingService>();
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreateDateRange();
            var reservationTime = UtcDateTime.Now;
            var pricingDetails = CreatePricingDetails(duration);
            pricingService
                .CalculatePrice(apartment, duration)
                .Returns(Result.Success(pricingDetails));

            var booking = Booking.Reserve(
                apartment,
                user,
                duration,
                reservationTime,
                pricingService
            );

            // Act
            Action act = () => booking.Confirm(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reject_ShouldFail_WhenStatusIsNotReserved()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.Confirm(UtcDateTime.Now);

            // Act
            var result = booking.Reject(UtcDateTime.Now);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Reject_ShouldRaise_BookingRejectedDomainEvent()
        {
            // Arrange
            var booking = CreateReservedBooking();
            booking.ClearDomainEvents();
            var rejectionTime = UtcDateTime.Now;

            // Act
            var result = booking.Reject(rejectionTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = booking.GetDomainEvents();
            events.Should().ContainSingle(e => e is BookingRejectedDomainEvent);
        }

        [Fact]
        public void Reject_ShouldSetStatusToRejected_WhenStatusIsReserved()
        {
            // Arrange
            var booking = CreateReservedBooking();
            var rejectionTime = UtcDateTime.Now;

            // Act
            var result = booking.Reject(rejectionTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            booking.Status.Should().Be(BookingStatus.Rejected);
            booking.RejectedOn.Should().Be(rejectionTime);
        }

        [Fact]
        public void Reject_ShouldThrow_WhenRejectionTimeIsNull()
        {
            // Arrange
            var booking = Booking.Reserve(
                CreateApartment(),
                CreateUser(),
                CreateDateRange(),
                UtcDateTime.Now,
                CreateDummyPricingService()
            );

            // Act
            Action act = () => booking.Reject(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reserve_ShouldCreateBookingWithReservedStatus()
        {
            // Arrange
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreateDateRange();
            var reservationTime = UtcDateTime.Now;

            var pricingService = Substitute.For<IPricingService>();
            var pricingDetails = CreatePricingDetails(duration);
            pricingService
                .CalculatePrice(apartment, duration)!
                .Returns(Result.Success(pricingDetails));

            // Act
            var booking = Booking.Reserve(
                apartment,
                user,
                duration,
                reservationTime,
                pricingService
            );

            // Assert
            booking.Status.Should().Be(BookingStatus.Reserved);
            booking.ApartmentId.Should().Be(apartment.Id);
            booking.UserId.Should().Be(user.Id);
            booking.Duration.Should().Be(duration);
            booking.CreatedOn.Should().Be(reservationTime);
            booking.TotalPrice.Should().Be(pricingDetails.TotalPrice);
        }

        [Fact]
        public void Reserve_ShouldRaise_BookingReservedDomainEvent()
        {
            // Arrange
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreateDateRange();
            var reservationTime = UtcDateTime.Now;
            var pricingService = Substitute.For<IPricingService>();
            var pricingDetails = CreatePricingDetails(duration);
            pricingService
                .CalculatePrice(apartment, duration)!
                .Returns(Result.Success(pricingDetails));

            // Act
            var booking = Booking.Reserve(
                apartment,
                user,
                duration,
                reservationTime,
                pricingService
            );

            // Assert
            var events = booking.GetDomainEvents();
            events.Should().ContainSingle(e => e is BookingReservedDomainEvent);
        }

        [Fact]
        public void Reserve_ShouldThrow_WhenApartmentIsNull()
        {
            // Arrange
            var user = CreateUser();
            var duration = CreateDateRange();
            var reservationTime = UtcDateTime.Now;
            var pricingService = CreateDummyPricingService();

            // Act
            Action act = () =>
                Booking.Reserve(null!, user, duration, reservationTime, pricingService);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reserve_ShouldThrow_WhenDurationIsNull()
        {
            // Arrange
            var apartment = CreateApartment();
            var user = CreateUser();
            var reservationTime = UtcDateTime.Now;
            var pricingService = CreateDummyPricingService();

            // Act
            Action act = () =>
                Booking.Reserve(apartment, user, null!, reservationTime, pricingService);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reserve_ShouldThrow_WhenPricingServiceIsNull()
        {
            // Arrange
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreateDateRange();
            var reservationTime = UtcDateTime.Now;

            // Act
            Action act = () => Booking.Reserve(apartment, user, duration, reservationTime, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reserve_ShouldThrow_WhenReservationTimeIsNull()
        {
            // Arrange
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreateDateRange();
            var pricingService = CreateDummyPricingService();

            // Act
            Action act = () => Booking.Reserve(apartment, user, duration, null!, pricingService);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        private static Apartment CreateApartment()
        {
            return new Apartment(
                Guid.NewGuid(),
                new Address("Street", "City", "State", "12345", "Country"),
                new Money(50, Currency.Usd),
                new Description("Nice place"),
                new Name("Test Apartment"),
                new Money(100, Currency.Usd),
                [Amenity.WiFi]
            );
        }

        private static DateRange CreateDateRange()
        {
            var start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var end = start.AddDays(3);
            return DateRange.Create(start, end);
        }

        private static IPricingService CreateDummyPricingService()
        {
            var pricingService = Substitute.For<IPricingService>();
            var duration = CreateDateRange();
            var pricingDetails = CreatePricingDetails(duration);
            pricingService
                .CalculatePrice(Arg.Any<Apartment>(), Arg.Any<DateRange>())
                .Returns(Result.Success(pricingDetails));
            return pricingService;
        }

        private static DateRange CreatePastDateRange()
        {
            var start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
            var end = start.AddDays(3);
            return DateRange.Create(start, end);
        }

        private static Booking CreatePastReservedBooking()
        {
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreatePastDateRange();
            var reservationTime = UtcDateTime.Now;

            var pricingService = Substitute.For<IPricingService>();
            var pricingDetails = CreatePricingDetails(duration);
            pricingService
                .CalculatePrice(apartment, duration)
                .Returns(Result.Success(pricingDetails));

            return Booking.Reserve(apartment, user, duration, reservationTime, pricingService);
        }

        private static PricingDetails CreatePricingDetails(DateRange period)
        {
            var priceForPeriod = new Money(100 * period.DurationInDays, Currency.Usd);
            var cleaningFee = new Money(50, Currency.Usd);
            var amenitiesUpCharge = new Money(20, Currency.Usd);
            var total = priceForPeriod + cleaningFee + amenitiesUpCharge;
            return new PricingDetails(priceForPeriod, cleaningFee, amenitiesUpCharge, total);
        }

        private static Booking CreateReservedBooking()
        {
            var apartment = CreateApartment();
            var user = CreateUser();
            var duration = CreateDateRange();
            var reservationTime = UtcDateTime.Now;

            var pricingService = Substitute.For<IPricingService>();
            var pricingDetails = CreatePricingDetails(duration);
            pricingService
                .CalculatePrice(apartment, duration)
                .Returns(Result.Success(pricingDetails));

            return Booking.Reserve(apartment, user, duration, reservationTime, pricingService);
        }

        private static User CreateUser()
        {
            return User.Create(
                new FirstName("Test"),
                new LastName("User"),
                new Email("test.user@example.com")
            );
        }
    }
}
