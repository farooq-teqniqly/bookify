using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events
{
    public sealed record UserCreatedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; }

        public UserCreatedDomainEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
