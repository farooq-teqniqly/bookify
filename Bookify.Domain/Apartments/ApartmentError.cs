using Bookify.Results;

namespace Bookify.Domain.Apartments
{
    public sealed class ApartmentError : Error
    {
        public ApartmentError(string code, string message, Guid apartmentId)
            : base(code, message, new Dictionary<string, object> { { "apartmentId", apartmentId } })
        {
            ApartmentId = apartmentId;
        }

        public Guid ApartmentId { get; }

        public static ApartmentError NotFound(Guid apartmentId) =>
            new("Apartment.NotFound", "The apartment was not found.", apartmentId);
    }
}
