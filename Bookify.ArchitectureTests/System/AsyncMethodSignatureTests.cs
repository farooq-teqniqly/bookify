using System.Reflection;
using Bookify.Application.Abstractions.Messaging;
using FluentAssertions;
using MediatR;

namespace Bookify.ArchitectureTests.System
{
    public class AsyncMethodSignatureTests
    {
        [Fact]
        public void All_Async_Methods_Should_Have_CancellationToken_With_Default_Value()
        {
            var assemblies = TestHelpers
                .GetBookifyAssemblies()
                .Where(a => a.GetName().Name != null && a.GetName().Name!.StartsWith("Bookify."))
                .ToList();

            var asyncMethods = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass || t.IsInterface)
                .SelectMany(t =>
                    t.GetMethods(
                        BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.DeclaredOnly
                    )
                )
                .Where(m =>
                    m.Name.EndsWith("Async")
                    && (
                        m.ReturnType == typeof(Task)
                        || (
                            m.ReturnType.IsGenericType
                            && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                        )
                    )
                )
                .ToList();

            foreach (var method in asyncMethods)
            {
                var ctParam = method
                    .GetParameters()
                    .FirstOrDefault(p => p.ParameterType == typeof(CancellationToken));

                ctParam
                    .Should()
                    .NotBeNull(
                        $"{method.DeclaringType?.FullName}.{method.Name} should have a CancellationToken parameter"
                    );

                ctParam!
                    .HasDefaultValue.Should()
                    .BeTrue(
                        $"{method.DeclaringType?.FullName}.{method.Name} CancellationToken should have a default value"
                    );

                // Accept both null and CancellationToken.None as valid for interface methods
                var isInterface = method.DeclaringType?.IsInterface == true;

                if (isInterface)
                {
                    (
                        ctParam.DefaultValue is null
                        || ctParam.DefaultValue?.Equals(CancellationToken.None) == true
                    )
                        .Should()
                        .BeTrue(
                            $"{method.DeclaringType?.FullName}.{method.Name} CancellationToken default should be 'default'"
                        );
                }
                else
                {
                    ctParam
                        .DefaultValue.Should()
                        .Be(
                            CancellationToken.None,
                            $"{method.DeclaringType?.FullName}.{method.Name} CancellationToken default should be 'default'"
                        );
                }
            }
        }

        [Fact]
        public void All_Methods_Returning_Task_Should_Have_Names_Ending_With_Async()
        {
            // Arrange & Act
            var handleMethodName = "Handle";

            var methodsReturningTask = TestHelpers
                .GetBookifyAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass || t.IsInterface)
                .Where(t =>
                    // Exclude classes implementing INotificationHandler<> with a Handle method
                    !t.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)
                        )
                    || !t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Any(m => m.Name == handleMethodName)
                )
                .Where(t =>
                    // Exclude classes implementing ICommandHandler<T1, T2> with a Handle method
                    !t.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                        )
                    || !t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Any(m => m.Name == handleMethodName)
                )
                .SelectMany(t =>
                    t.GetMethods(
                        BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.DeclaredOnly
                    )
                )
                .Where(m =>
                    m.ReturnType == typeof(Task)
                    || (
                        m.ReturnType.IsGenericType
                        && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                    )
                )
                .ToList();

            var invalidMethods = methodsReturningTask
                .Where(m => !m.Name.EndsWith("Async"))
                .Select(m => $"{m.DeclaringType?.FullName}.{m.Name}")
                .ToList();

            // Assert
            invalidMethods
                .Should()
                .BeEmpty(
                    "The following methods return Task or Task<T> but do not have names ending with 'Async':\n"
                        + string.Join("\n", invalidMethods)
                );
        }
    }
}
