using CodEaisy.TinySaas.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Core
{
    /// <summary>
    /// Configure tenant services
    /// </summary>
    public class TenantBuilder<T> where T : ITenant
    {
        private readonly IServiceCollection _services;

        public TenantBuilder(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.Add(ServiceDescriptor.Describe(typeof(ITenantContextService<T>), typeof(TenantContextService<T>), ServiceLifetime.Transient));
            _services = services;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lifetime"></param>
        internal TenantBuilder<T> WithResolutionStrategy<V>(ServiceLifetime lifetime = ServiceLifetime.Transient) where V : ITenantResolutionStrategy
        {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(V), lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        internal TenantBuilder<T> WithStore<V>(ServiceLifetime lifetime = ServiceLifetime.Transient) where V : ITenantStore<T>
        {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(V), lifetime));
            return this;
        }
    }
}
