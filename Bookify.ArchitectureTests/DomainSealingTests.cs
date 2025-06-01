using Bookify.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests
{
    public class DomainSealingTests
    {
        [Fact]
        public void DomainClasses_And_Records_Should_Be_Sealed()
        {
            // Arrange & Act
            var result = Types
                .InAssembly(typeof(Entity).Assembly)
                .That()
                .DoNotResideInNamespace("Bookify.Domain.Abstractions")
                .And()
                .AreNotInterfaces()
                .Should()
                .BeSealed()
                .GetResult();

            // Assert
            var failingTypeNames = result.FailingTypeNames;

            failingTypeNames?.Should().HaveCount(0);
        }
    }
}
