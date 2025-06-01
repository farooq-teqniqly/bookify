using System.Reflection;
using FluentAssertions;

namespace Bookify.ArchitectureTests.System
{
    public class AsyncMethodSignatureTests
    {
        [Fact]
        public void All_Async_Methods_Should_Have_CancellationToken_With_Default_Value()
        {
            var assemblies = AppDomain
                .CurrentDomain.GetAssemblies()
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
    }
}
