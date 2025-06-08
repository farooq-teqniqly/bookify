using Bookify.Results;

namespace Bookify.Application.Abstractions
{
    public interface IEmailService
    {
        Task<Result<Unit>> SendAsync(
            Domain.Users.Email recipient,
            string subject,
            string body,
            CancellationToken cancellationToken = default
        );
    }
}
