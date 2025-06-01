using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using FluentAssertions;

namespace Bookify.Domain.Tests.Users
{
    public class UserTests
    {
        [Fact]
        public void Create_ShouldRaiseUserCreatedDomainEvent()
        {
            // Arrange
            var firstName = new FirstName("Jane");
            var lastName = new LastName("Smith");
            var email = new Email("jane.smith@example.com");

            // Act
            var user = User.Create(firstName, lastName, email);
            var events = user.GetDomainEvents();

            // Assert
            events
                .Should()
                .ContainSingle()
                .Which.Should()
                .BeOfType<UserCreatedDomainEvent>()
                .Which.UserId.Should()
                .Be(user.Id);
        }

        [Fact]
        public void Create_ShouldReturnUserWithCorrectProperties()
        {
            // Arrange
            var firstName = new FirstName("John");
            var lastName = new LastName("Doe");
            var email = new Email("john.doe@example.com");

            // Act
            var user = User.Create(firstName, lastName, email);

            // Assert
            user.Should().NotBeNull();
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Email.Should().Be(email);
            user.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenEmailIsNull()
        {
            // Arrange
            FirstName firstName = new FirstName("John");
            LastName lastName = new LastName("Doe");

            // Act
            Action act = () => User.Create(firstName, lastName, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenFirstNameIsNull()
        {
            // Arrange
            LastName lastName = new LastName("Doe");
            Email email = new Email("john.doe@example.com");

            // Act
            Action act = () => User.Create(null!, lastName, email);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenLastNameIsNull()
        {
            // Arrange
            FirstName firstName = new FirstName("John");
            Email email = new Email("john.doe@example.com");

            // Act
            Action act = () => User.Create(firstName, null!, email);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
