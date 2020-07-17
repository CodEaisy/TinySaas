using System.Threading.Tasks;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Resolvers
{
    /// <summary>
    /// <para>Resolve an header entry to a tenant identifier.</para>
    /// <para>Uses CodEaisy.TinySaas.MultitenancyConstants.TenantIdKey as key</para>
    /// </summary>
    public class HeaderResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public HeaderResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <returns>tenant identifier</returns>
        public ValueTask<string> GetTenantIdentifierAsync()
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(MultitenancyConstants.TenantIdKey, out var tenantId);
            return new ValueTask<string>(tenantId);
        }
    }
}
