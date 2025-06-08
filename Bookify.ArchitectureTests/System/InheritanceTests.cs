using FluentAssertions;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests.System
{
    public class InheritanceTests
    {
        [Fact]
        public void Classes_And_Records_Outside_Domain_Abstractions_Should_Be_Sealed()
        {
            // Arrange & Act
            var result = Types
                .InAssemblies(TestHelpers.GetBookifyAssemblies())
                .That()
                .DoNotResideInNamespace("Bookify.Domain.Abstractions")
                .And()
                .AreNotInterfaces()
                .And()
                .AreNotAbstract()
                .Should()
                .BeSealed()
                .GetResult();

            // Assert
            var failingTypeNames = result.FailingTypeNames;

            failingTypeNames.Should().BeNullOrEmpty();
        }
    }
}
