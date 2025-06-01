using System.Reflection;
using Bookify.Domain.Abstractions;
using Bookify.Results;
using FluentAssertions;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests.Domain
{
    public class DomainArchitectureTests
    {
        [Fact]
        public void Domain_Classes_And_Records_Outside_Abstractions_Should_Be_Sealed()
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

            failingTypeNames.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Domain_Event_Class_Names_Should_End_With_DomainEvent()
        {
            // Arrange & Act
            var result = Types
                .InAssembly(typeof(Entity).Assembly)
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
                .InAssembly(typeof(Entity).Assembly)
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
        public void Repository_Interface_Methods_Should_Return_Result_Or_Result_Of_T()
        {
            AssertInterfaceMethodsReturnResultOrResultOfT("Repository");
        }

        [Fact]
        public void Service_Interface_Methods_Should_Return_Result_Or_Result_Of_T()
        {
            AssertInterfaceMethodsReturnResultOrResultOfT("Service");
        }

        [Fact]
        public void All_Domain_Classes_And_Records_Should_Have_Readonly_Or_InternalSet_Properties()
        {
            var domainTypes = typeof(Entity)
                .Assembly.GetTypes()
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

        private static void AssertInterfaceMethodsReturnResultOrResultOfT(string interfaceSuffix)
        {
            var interfaces = typeof(Entity)
                .Assembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.EndsWith(interfaceSuffix))
                .ToList();

            foreach (var @interface in interfaces)
            {
                foreach (var method in @interface.GetMethods())
                {
                    var returnType = method.ReturnType;
                    var methodName = method.Name;

                    if (methodName.EndsWith("Async"))
                    {
                        // Should return Task<Result<T>>
                        returnType
                            .IsGenericType.Should()
                            .BeTrue(
                                $"{@interface.Name}.{methodName} should return Task<Result<T>>"
                            );

                        returnType
                            .GetGenericTypeDefinition()
                            .Should()
                            .Be(
                                typeof(Task<>),
                                $"{@interface.Name}.{methodName} should return Task<Result<T>>"
                            );

                        var taskResultType = returnType.GetGenericArguments()[0];

                        taskResultType
                            .IsGenericType.Should()
                            .BeTrue(
                                $"{@interface.Name}.{methodName} should return Task<Result<T>>"
                            );

                        taskResultType
                            .GetGenericTypeDefinition()
                            .Should()
                            .Be(
                                typeof(Result<>),
                                $"{@interface.Name}.{methodName} should return Task<Result<T>>"
                            );
                    }
                    else
                    {
                        // Should return Result<T>
                        returnType
                            .IsGenericType.Should()
                            .BeTrue($"{@interface.Name}.{methodName} should return Result<T>");

                        returnType
                            .GetGenericTypeDefinition()
                            .Should()
                            .Be(
                                typeof(Result<>),
                                $"{@interface.Name}.{methodName} should return Result<T>"
                            );
                    }
                }
            }
        }
    }
}
