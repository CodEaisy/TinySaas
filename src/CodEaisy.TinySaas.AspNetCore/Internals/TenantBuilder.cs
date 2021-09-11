using CodEaisy.TinySaas.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Internals
{
    /// <summary>
    /// Configure tenant services
    /// </summary>
    internal class TenantBuilder<T> where T : class, ITenant
    {
        protected readonly IServiceCollection _services;

        public TenantBuilder(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<ITenantService<T>, TenantService<T>>();
            _services = services;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        public TenantBuilder<T> WithResolutionStrategy<V>() where V : class,ITenantResolutionStrategy
        {
            _services.AddSingleton<ITenantResolutionStrategy, V>();
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        public TenantBuilder<T> WithStore<V>() where V : class, ITenantStore<T>
        {
            _services.AddSingleton<ITenantStore<T>, V>();
            return this;
        }
    }
}
