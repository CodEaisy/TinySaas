using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Internals;

namespace CodEaisy.TinySaas
{
    /// <summary>
    /// Nice method to create the tenant builder
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services (application specific tenant class)
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMultitenancy<TTenant, TTenantStore, TResolutionStrategy>(this IServiceCollection services)
            where TTenant : class, ITenant
            where TTenantStore : class, ITenantStore<TTenant>
            where TResolutionStrategy : class, ITenantResolutionStrategy
        {
            services.AddSingleton<ITenant>(provider => provider
                .GetRequiredService<IHttpContextAccessor>()
                .HttpContext
                .GetCurrentTenant<TTenant>());
            services.AddSingleton(provider => (TTenant) provider.GetRequiredService<ITenant>());

            var tenantBuilder = new TenantBuilder<TTenant>(services);
            tenantBuilder.WithStore<TTenantStore>();
            tenantBuilder.WithResolutionStrategy<TResolutionStrategy>();
            return services;
        }
    }
}
