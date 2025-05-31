using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public record PricingDetails
{
    public Money PriceForPeriod { get; }
    public Money ApartmentCleaningFee { get; }
    public Money AmenitiesUpCharge { get; }
    public Money TotalPrice { get; }

    public PricingDetails(
        Money PriceForPeriod,
        Money ApartmentCleaningFee,
        Money AmenitiesUpCharge,
        Money TotalPrice
    )
    {
        ArgumentNullException.ThrowIfNull(PriceForPeriod);
        ArgumentNullException.ThrowIfNull(ApartmentCleaningFee);
        ArgumentNullException.ThrowIfNull(AmenitiesUpCharge);
        ArgumentNullException.ThrowIfNull(TotalPrice);

        this.PriceForPeriod = PriceForPeriod;
        this.ApartmentCleaningFee = ApartmentCleaningFee;
        this.AmenitiesUpCharge = AmenitiesUpCharge;
        this.TotalPrice = TotalPrice;
    }
}
