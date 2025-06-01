using Bookify.Domain.Apartments;
using Bookify.Results;

namespace Bookify.Domain.Bookings;

public interface IPricingService
{
    Result<PricingDetails> CalculatePrice(Apartment apartment, DateRange period);
}
