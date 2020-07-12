using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Core.Resolvers
{
    /// <summary>
    /// <para>Resolve an header entry to a tenant identifier.</para>
    /// <para>Uses CodEaisy.TinySaas.MultiTenancyConstants.TenantIdKey as key</para>
    /// </summary>
    public class HeaderResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        public ValueTask<string> GetTenantIdentifierAsync()
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(MultiTenancyConstants.TenantIdKey, out var tenantId);
            return new ValueTask<string>(tenantId);
        }
    }
}
