using Bookify.Results;

namespace Bookify.Domain.Users
{
    public interface IUserRepository
    {
        Result<Unit> Add(User user);
        Task<Result<User>> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
