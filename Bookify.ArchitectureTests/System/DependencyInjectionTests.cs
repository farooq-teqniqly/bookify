using System.Reflection;
using FluentAssertions;

namespace Bookify.ArchitectureTests.System
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void Service_And_Repository_Constructors_Should_Only_Accept_Interfaces()
        {
            // Arrange
            var assemblies = TestHelpers.GetBookifyAssemblies();

            var serviceAndRepositoryClasses = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    t is { IsClass: true, IsAbstract: false }
                    && (t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                )
                .ToList();

            var violations = new List<string>();

            foreach (var type in serviceAndRepositoryClasses)
            {
                var constructors = type.GetConstructors(
                    BindingFlags.Public | BindingFlags.Instance
                );

                foreach (var constructor in constructors)
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (!parameter.ParameterType.IsInterface)
                        {
                            violations.Add(
                                $"{type.FullName}.{constructor.Name} has a parameter '{parameter.Name}' of type '{parameter.ParameterType.FullName}', which is not an interface."
                            );
                        }
                    }
                }
            }

            // Assert
            violations
                .Should()
                .BeEmpty(
                    "All constructors in Service and Repository classes should only accept interfaces as parameters, but found:\n"
                        + string.Join("\n", violations)
                );
        }
    }
}
