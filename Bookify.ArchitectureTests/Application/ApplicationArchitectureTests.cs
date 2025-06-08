using System.Reflection;
using Bookify.Application.Abstractions.Messaging;
using FluentAssertions;
using MediatR;

namespace Bookify.ArchitectureTests.Application
{
    public class ApplicationArchitectureTests
    {
        private static readonly Assembly _applicationAssembly = Assembly.Load(
            "Bookify.Application"
        );

        [Fact]
        public void All_ICommandHandler_Generic_Parameters_Should_End_With_Command()
        {
            // Act
            var commandHandlerTypes = _applicationAssembly
                .GetTypes()
                .Where(type => type is { IsClass: true, IsAbstract: false })
                .Where(type =>
                    type.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                        )
                )
                .ToList();

            var invalidGenericParameters = new List<string>();

            foreach (var type in commandHandlerTypes)
            {
                var commandHandlerInterface = type.GetInterfaces()
                    .First(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                    );

                var genericArguments = commandHandlerInterface.GetGenericArguments();
                var tCommandParameter = genericArguments[0]; // TRequest is the first generic parameter

                if (!tCommandParameter.Name.EndsWith("Command"))
                {
                    invalidGenericParameters.Add($"{type.Name} -> {tCommandParameter.Name}");
                }
            }

            // Assert
            invalidGenericParameters
                .Should()
                .BeEmpty(
                    "The following ICommandHandler implementations have TCommandParameter that does not end with 'Command': {0}",
                    string.Join(", ", invalidGenericParameters)
                );
        }

        [Fact]
        public void All_ICommandHandler_Implementations_Should_End_With_CommandHandler()
        {
            // Act
            var commandHandlerTypes = _applicationAssembly
                .GetTypes()
                .Where(type => type is { IsClass: true, IsAbstract: false })
                .Where(type =>
                    type.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                        )
                )
                .ToList();

            var invalidTypes = commandHandlerTypes
                .Where(type => !type.Name.EndsWith("CommandHandler"))
                .ToList();

            // Assert
            invalidTypes
                .Should()
                .BeEmpty(
                    "The following ICommandHandler implementations do not end with 'CommandHandler': {0}",
                    string.Join(", ", invalidTypes.Select(t => t.Name))
                );
        }

        [Fact]
        public void All_INotificationHandler_Implementations_Should_End_With_EventHandler()
        {
            // Act
            var notificationHandlerTypes = _applicationAssembly
                .GetTypes()
                .Where(type => type is { IsClass: true, IsAbstract: false })
                .Where(type =>
                    type.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)
                        )
                )
                .ToList();

            var invalidTypes = notificationHandlerTypes
                .Where(type => !type.Name.EndsWith("EventHandler"))
                .ToList();

            // Assert
            invalidTypes
                .Should()
                .BeEmpty(
                    "The following INotificationHandler implementations do not end with 'EventHandler': {0}",
                    string.Join(", ", invalidTypes.Select(t => t.Name))
                );
        }

        [Fact]
        public void Application_Assembly_Should_Only_Depend_On_Bookify_Domain_And_Results_Assembly()
        {
            // Arrange
            var allowedBookifyDependencies = new[]
            {
                "Bookify.Application",
                "Bookify.Domain",
                "Bookify.Results",
            };

            var referencedAssemblies = _applicationAssembly
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
                    "Bookify.Domain should only depend on Bookify.Domain, Bookify.Results (and itself), but found:\n"
                        + string.Join("\n", disallowed)
                );
        }
    }
}
