using System.Threading.Tasks;
using CodEaisy.TinySaas.Interfaces;

namespace CodEaisy.TinySaas.Internals
{
    /// <summary>
    /// Tenant service
    /// </summary>
    /// <typeparam name="T">type of tenant</typeparam>
    public class TenantService<T> : ITenantService<T> where T : class, ITenant
    {
        /// <summary>
        /// tenant resolution strategy
        /// </summary>
        protected readonly ITenantResolutionStrategy _tenantResolutionStrategy;

        /// <summary>
        /// tenant storage engine
        /// </summary>
        protected readonly ITenantStore<T> _tenantStore;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="tenantResolutionStrategy"></param>
        /// <param name="tenantStore"></param>
        public TenantService(ITenantResolutionStrategy tenantResolutionStrategy, ITenantStore<T> tenantStore)
        {
            _tenantResolutionStrategy = tenantResolutionStrategy;
            _tenantStore = tenantStore;
        }

        /// <summary>
        /// Get the current tenant
        /// </summary>
        /// <returns>tenant</returns>
        public async Task<T> GetTenant()
        {
            var tenantIdentifier = await _tenantResolutionStrategy.GetTenantIdentifierAsync();
            return await _tenantStore.GetTenant(tenantIdentifier);
        }
    }
}
