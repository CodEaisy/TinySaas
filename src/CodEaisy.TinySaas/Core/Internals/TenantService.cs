using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;

namespace CodEaisy.TinySaas.Core.Internals
{
    /// <summary>
    /// Tenant service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TenantService<T> : ITenantService<T> where T : class, ITenant
    {
        private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
        private readonly ITenantStore<T> _tenantStore;

        public TenantService(ITenantResolutionStrategy tenantResolutionStrategy, ITenantStore<T> tenantStore)
        {
            _tenantResolutionStrategy = tenantResolutionStrategy;
            _tenantStore = tenantStore;
        }

        /// <summary>
        /// Get the current tenant
        /// </summary>
        public async Task<T> GetTenant()
        {
            var tenantIdentifier = await _tenantResolutionStrategy.GetTenantIdentifierAsync();
            return await _tenantStore.GetTenant(tenantIdentifier);
        }
    }
}
