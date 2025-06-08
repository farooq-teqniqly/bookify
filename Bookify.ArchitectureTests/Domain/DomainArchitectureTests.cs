using System.Reflection;
using Bookify.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests.Domain
{
    public class DomainArchitectureTests
    {
        private static readonly Assembly _domainAssembly = Assembly.Load("Bookify.Domain");

        [Fact]
        public void All_Domain_Classes_And_Records_Should_Have_Readonly_Or_InternalSet_Properties()
        {
            var domainTypes = _domainAssembly
                .GetTypes()
                .Where(t => t.IsClass)
                .Where(t =>
                    t.Namespace != null
                    && t.Namespace.StartsWith("Bookify.Domain")
                    && t.Namespace != "Bookify.Domain.Abstractions"
                );

            var violations = new List<string>();

            foreach (var type in domainTypes)
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in properties)
                {
                    var setMethod = prop.SetMethod;
                    // Allow if no setter, or if setter is internal (IsAssembly), but not if public
                    var isAllowed = setMethod == null || setMethod.IsAssembly;

                    if (!isAllowed)
                    {
                        violations.Add(
                            $"{type.FullName}.{prop.Name} should be readonly or have an internal setter"
                        );
                    }
                }
            }

            violations
                .Should()
                .BeEmpty(
                    "All domain properties should be readonly or have an internal setter, but found:\n"
                        + string.Join("\n", violations)
                );
        }

        [Fact]
        public void Domain_Assembly_Should_Only_Depend_On_Bookify_Results_Assembly()
        {
            // Arrange
            var allowedBookifyDependencies = new[] { "Bookify.Domain", "Bookify.Results" };

            var referencedAssemblies = _domainAssembly
                .GetReferencedAssemblies()
                .Select(a => a.Name)
                .Where(name => name!.StartsWith("Bookify"))
                .ToList();

            var disallowed = referencedAssemblies
                .Where(name => !allowedBookifyDependencies.Contains(name))
                .ToList();

            disallowed
                .Should()
                .BeEmpty(
                    "Bookify.Domain should only depend on Bookify.Results (and itself), but found:\n"
                        + string.Join("\n", disallowed)
                );
        }

        [Fact]
        public void Domain_Event_Class_Names_Should_End_With_DomainEvent()
        {
            // Arrange & Act
            var result = Types
                .InAssembly(_domainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .HaveNameEndingWith("DomainEvent")
                .GetResult();

            // Assert
            var failingTypeNames = result.FailingTypeNames;

            failingTypeNames.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Domain_Event_Classes_Should_Reside_In_Events_Namespace()
        {
            // Arrange & Act
            var result = Types
                .InAssembly(_domainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .ResideInNamespaceEndingWith("Events")
                .GetResult();

            // Assert
            var failingTypeNames = result.FailingTypeNames;

            failingTypeNames.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Repository_Interfaces_Should_Only_Be_Defined_In_Bookify_Domain_Assembly()
        {
            // Arrange
            var repositoryInterfaces = TestHelpers
                .GetBookifyAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsInterface && type.Name.EndsWith("Repository"))
                .ToList();

            var invalidRepositories = repositoryInterfaces
                .Where(type => type.Assembly != _domainAssembly)
                .Select(type => $"{type.FullName} (in {type.Assembly.GetName().Name})")
                .ToList();

            // Assert
            invalidRepositories
                .Should()
                .BeEmpty(
                    "repository interfaces should only be defined in the Bookify.Domain assembly, but found:\n"
                        + string.Join("\n", invalidRepositories)
                );
        }
    }
}
