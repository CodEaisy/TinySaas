using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;
using CodEaisy.TinySaas.Model;

namespace CodEaisy.TinySaas.Core
{
    /// <summary>
    /// Tenant context service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TenantContextService<T> : ITenantContextService<T> where T : ITenant
    {
        private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
        private readonly ITenantStore<T> _tenantStore;

        public TenantContextService(ITenantResolutionStrategy tenantResolutionStrategy, ITenantStore<T> tenantStore)
        {
            _tenantResolutionStrategy = tenantResolutionStrategy;
            _tenantStore = tenantStore;
        }

        /// <summary>
        /// Get the current tenant
        /// </summary>
        public async Task<ITenantContext<T>> GetTenantContext()
        {
            var tenantIdentifier = await _tenantResolutionStrategy.GetTenantIdentifierAsync();
            var tenant = await _tenantStore.GetTenant(tenantIdentifier);

            return tenant == null ? null : new TenantContext<T>(tenant);
        }
    }
}
