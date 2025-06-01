namespace Bookify.Domain.Abstractions
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be the empty guid.");
            }

            Id = id;
        }

        public Guid Id { get; init; }

        public void ClearDomainEvents() => _domainEvents.Clear();

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }
}
