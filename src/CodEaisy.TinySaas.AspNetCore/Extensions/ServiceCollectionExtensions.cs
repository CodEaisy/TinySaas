using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton(provider => (TTenant) provider.GetRequiredService<ITenant>())
                .AddTenantBuilder<TTenant>()
                .WithStore<TTenantStore>()
                .WithResolutionStrategy<TResolutionStrategy>();

            return services;
        }
        
        private static TenantBuilder<TTenant> AddTenantBuilder<TTenant>(this IServiceCollection services)
            where TTenant: class, ITenant
            => new TenantBuilder<TTenant>(services); 
    }
}
