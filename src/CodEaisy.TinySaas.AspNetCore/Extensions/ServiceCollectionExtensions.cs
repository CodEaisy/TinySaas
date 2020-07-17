using CodEaisy.TinySaas.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
            where TTenantStore : ITenantStore<TTenant>
            where TResolutionStrategy : ITenantResolutionStrategy
        {
            services.AddScoped<ITenantAccessor<TTenant>, TenantAccessor<TTenant>>();

            var tenantBuilder = new TenantBuilder<TTenant>(services);
            tenantBuilder.WithStore<TTenantStore>();
            tenantBuilder.WithResolutionStrategy<TResolutionStrategy>();
            return services;
        }
    }
}
