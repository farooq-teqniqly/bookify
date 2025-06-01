using Bookify.Results;

namespace Bookify.Domain.Apartments
{
    public interface IApartmentRepository
    {
        Task<Result<Apartment>> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
