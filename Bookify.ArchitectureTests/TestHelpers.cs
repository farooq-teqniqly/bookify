using System.Reflection;
using Bookify.Results;
using FluentAssertions;

namespace Bookify.ArchitectureTests
{
    internal static class TestHelpers
    {
        internal static void AssertInterfaceMethodsReturnResultOrResultOfT(string interfaceSuffix)
        {
            var interfaces = GetBookifyAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
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

        internal static List<Assembly> GetBookifyAssemblies()
        {
            // Specify the assemblies to load
            var targetAssemblies = new[]
            {
                "Bookify.Domain",
                "Bookify.Application",
                "Bookify.Results",
            };

            var loadedAssemblies = AppDomain
                .CurrentDomain.GetAssemblies()
                .Where(assembly => targetAssemblies.Contains(assembly.GetName().Name))
                .ToList();

            var loadedAssemblyNames = loadedAssemblies.Select(a => a.GetName().Name).ToHashSet();

            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyDirectory == null)
            {
                return loadedAssemblies;
            }

            var allAssemblyFiles = Directory.GetFiles(assemblyDirectory, "*.dll");

            foreach (var assemblyFile in allAssemblyFiles)
            {
                var assemblyName = AssemblyName.GetAssemblyName(assemblyFile).Name;
                if (
                    targetAssemblies.Contains(assemblyName)
                    && !loadedAssemblyNames.Contains(assemblyName)
                )
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    loadedAssemblies.Add(assembly);
                }
            }

            targetAssemblies
                .Should()
                .HaveCount(
                    targetAssemblies.Length,
                    "All target assemblies should be loaded into the current AppDomain"
                );

            return loadedAssemblies;
        }
    }
}
