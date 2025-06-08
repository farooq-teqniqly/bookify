using Bookify.Application.Abstractions;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Results;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPricingService _pricingService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ReserveBookingCommandHandler(
        IPricingService pricingService,
        IApartmentRepository apartmentRepository,
        IUserRepository userRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider
    )
    {
        _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));

        _apartmentRepository =
            apartmentRepository ?? throw new ArgumentNullException(nameof(apartmentRepository));

        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        _bookingRepository =
            bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));

        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        _dateTimeProvider =
            dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<Result<Guid>> Handle(
        ReserveBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var getApartmentResult = await _apartmentRepository
            .GetByIdAsync(request.ApartmentId, cancellationToken)
            .ConfigureAwait(false);

        if (getApartmentResult.IsFailure)
        {
            return Result.Failure<Guid>(getApartmentResult.GetError());
        }

        var getUserResult = await _userRepository
            .GetByIdAsync(request.UserId, cancellationToken)
            .ConfigureAwait(false);

        if (getUserResult.IsFailure)
        {
            return Result.Failure<Guid>(getUserResult.GetError());
        }

        var apartment = getApartmentResult.GetValue();
        var user = getUserResult.GetValue();
        var duration = DateRange.Create(request.StartDate, request.EndDate);

        var isOverlapResult = await _bookingRepository
            .IsOverlappingAsync(apartment, duration, cancellationToken)
            .ConfigureAwait(false);

        if (isOverlapResult.IsFailure)
        {
            return Result.Failure<Guid>(isOverlapResult.GetError());
        }

        var booking = Booking.Reserve(
            apartment,
            user,
            duration,
            _dateTimeProvider.Now,
            _pricingService
        );

        await _bookingRepository.AddBookingAsync(booking, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return Result.Success(booking.Id);
    }
}
