using CodEaisy.TinySaas.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Core.Internals
{
    /// <summary>
    /// Configure tenant services
    /// </summary>
    public class TenantBuilder<T> where T : class, ITenant
    {
        private readonly IServiceCollection _services;

        public TenantBuilder(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.Add(ServiceDescriptor.Describe(typeof(ITenantService<T>), typeof(TenantService<T>), ServiceLifetime.Scoped));
            _services = services;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lifetime"></param>
        public TenantBuilder<T> WithResolutionStrategy<V>(ServiceLifetime lifetime = ServiceLifetime.Transient) where V : ITenantResolutionStrategy
        {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(V), lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lifetime"></param>
        public TenantBuilder<T> WithStore<V>(ServiceLifetime lifetime = ServiceLifetime.Singleton) where V : ITenantStore<T>
        {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(V), lifetime));
            return this;
        }
    }
}
