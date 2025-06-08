using Bookify.Results;

namespace Bookify.Domain.Users
{
    public sealed class UserError : Error
    {
        public UserError(string code, string message, Guid userId)
            : base(code, message, new Dictionary<string, object> { { "userId", userId } })
        {
            UserId = userId;
        }

        public Guid UserId { get; }

        public static UserError NotFound(Guid userId) =>
            new("User.NotFound", "The user was not found.", userId);
    }
}
