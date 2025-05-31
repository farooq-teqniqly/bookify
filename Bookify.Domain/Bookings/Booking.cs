using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;
using Bookify.Results;

namespace Bookify.Domain.Bookings
{
    public sealed class Booking : Entity
    {
        private Booking(
            Guid id,
            Guid apartmentId,
            Guid userId,
            DateRange duration,
            Money priceForPeriod,
            Money cleaningFee,
            Money amenitiesUpCharge,
            Money totalPrice,
            BookingStatus status,
            UtcDateTime createdOn
        )
            : base(id)
        {
            AmenitiesUpCharge = amenitiesUpCharge;
            CleaningFee = cleaningFee;
            CreatedOn = createdOn;
            Duration = duration;
            PriceForPeriod = priceForPeriod;
            TotalPrice = totalPrice;
            ApartmentId = apartmentId;
            Status = status;
            UserId = userId;
        }

        public Money AmenitiesUpCharge { get; private set; }
        public Guid ApartmentId { get; private set; }
        public UtcDateTime? CancelledOn { get; private set; }
        public Money CleaningFee { get; private set; }
        public UtcDateTime? CompletedOn { get; private set; }
        public UtcDateTime? ConfirmedOn { get; private set; }
        public UtcDateTime CreatedOn { get; private set; }
        public DateRange Duration { get; private set; }
        public Money PriceForPeriod { get; private set; }
        public UtcDateTime? RejectedOn { get; private set; }
        public BookingStatus Status { get; private set; }
        public Money TotalPrice { get; private set; }
        public Guid UserId { get; private set; }

        public static Booking Reserve(
            Apartment apartment,
            User user,
            DateRange duration,
            UtcDateTime reservationTime,
            IPricingService pricingService
        )
        {
            ArgumentNullException.ThrowIfNull(apartment);
            ArgumentNullException.ThrowIfNull(duration);
            ArgumentNullException.ThrowIfNull(pricingService);
            ArgumentNullException.ThrowIfNull(reservationTime);

            var pricingDetails = pricingService.CalculatePrice(apartment, duration);

            var booking = new Booking(
                Guid.NewGuid(),
                apartment.Id,
                user.Id,
                duration,
                pricingDetails.PriceForPeriod,
                pricingDetails.ApartmentCleaningFee,
                pricingDetails.AmenitiesUpCharge,
                pricingDetails.TotalPrice,
                BookingStatus.Reserved,
                reservationTime
            );

            booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

            apartment.LastBookedOn = UtcDateTime.Now;

            return booking;
        }

        public Result<Unit> Cancel(UtcDateTime cancelTime)
        {
            ArgumentNullException.ThrowIfNull(cancelTime);

            if (Status != BookingStatus.Confirmed)
            {
                return Result.Failure<Unit>(BookingError.NotConfirmed(Id));
            }

            var currentDate = cancelTime.GetDate();

            if (currentDate > Duration.Start)
            {
                return Result.Failure<Unit>(BookingError.AlreadyStarted(Id));
            }

            Status = BookingStatus.Cancelled;
            CancelledOn = cancelTime;
            RaiseDomainEvent(new BookingCancelledDomainEvent(Id));

            return Result.Success(Unit.Value);
        }

        public Result<Unit> Complete(UtcDateTime completionTime)
        {
            ArgumentNullException.ThrowIfNull(completionTime);

            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure<Unit>(BookingError.NotReserved(Id));
            }

            Status = BookingStatus.Completed;
            CompletedOn = completionTime;
            RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

            return Result.Success(Unit.Value);
        }

        public Result<Unit> Confirm(UtcDateTime confirmationTime)
        {
            ArgumentNullException.ThrowIfNull(confirmationTime);

            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure<Unit>(BookingError.NotReserved(Id));
            }

            Status = BookingStatus.Confirmed;
            ConfirmedOn = confirmationTime;
            RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

            return Result.Success(Unit.Value);
        }

        public Result<Unit> Reject(UtcDateTime rejectionTime)
        {
            ArgumentNullException.ThrowIfNull(rejectionTime);

            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure<Unit>(BookingError.NotReserved(Id));
            }

            Status = BookingStatus.Rejected;
            RejectedOn = rejectionTime;
            RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

            return Result.Success(Unit.Value);
        }
    }
}
