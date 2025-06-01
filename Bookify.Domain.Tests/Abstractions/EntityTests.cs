using Bookify.Domain.Abstractions;
using FluentAssertions;

namespace Bookify.Domain.Tests.Abstractions
{
    public class EntityTests
    {
        [Fact]
        public void ClearDomainEvents_RemovesAllDomainEvents()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid());
            entity.AddDomainEvent(new TestDomainEvent());
            entity.AddDomainEvent(new TestDomainEvent());

            // Act
            entity.ClearDomainEvents();

            // Assert
            entity.GetDomainEvents().Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithEmptyGuid_ThrowsArgumentException()
        {
            // Act
            Action act = () => new TestEntity(Guid.Empty);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Id cannot be the empty guid.");
        }

        [Fact]
        public void Constructor_WithValidGuid_SetsId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var entity = new TestEntity(id);

            // Assert
            entity.Id.Should().Be(id);
        }

        [Fact]
        public void RaiseDomainEvent_AddsEventToDomainEvents()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid());
            var domainEvent = new TestDomainEvent();

            // Act
            entity.AddDomainEvent(domainEvent);

            // Assert
            entity.GetDomainEvents().Should().ContainSingle().Which.Should().Be(domainEvent);
        }
    }

    // Dummy domain event for testing
    public class TestDomainEvent : IDomainEvent { }

    // Concrete implementation for testing
    public class TestEntity : Entity
    {
        public TestEntity(Guid id)
            : base(id) { }

        public void AddDomainEvent(IDomainEvent domainEvent) => RaiseDomainEvent(domainEvent);
    }
}
