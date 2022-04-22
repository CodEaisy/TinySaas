using System;
using System.Collections.Generic;
using System.Linq;
using CodEaisy.TinySaas.Samples.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Tests.Helpers
{
    public static class AppFactoryExtensions
    {
        public static WebApplicationFactory<Startup> UpdateDependencyInjection(
            this WebApplicationFactory<Startup> factory, List<DependencyUpdate> dependencies) =>
            factory.WithWebHostBuilder(builder => {
                builder.ConfigureTestServices(services => {
                    foreach (var dep in dependencies)
                    {
                        var existingService = services.SingleOrDefault(d => d.ServiceType == dep.Definition);
                        services.Add(new ServiceDescriptor(dep.Definition, dep.Implementation,
                            existingService.Lifetime));
                    }
                });
            });
    }

    public class DependencyUpdate
    {
        public DependencyUpdate(Type definition, Type implementation)
        {
            if (implementation.GetInterface(definition.FullName) == null)
                throw new ArgumentException("incompatible types");
            Definition = definition;
            Implementation = implementation;
        }

        public Type Definition { get; }
        public Type Implementation { get; }
    }
}
