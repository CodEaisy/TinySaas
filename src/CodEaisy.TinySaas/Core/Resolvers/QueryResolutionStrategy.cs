using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Core.Resolvers
{
    /// <summary>
    /// <para>Resolve a query parameter to a tenant identifier.</para>
    /// <para>Uses CodEaisy.TinySaas.MultiTenancyConstants.TenantIdKey as key</para>
    /// </summary>
    public class QueryResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QueryResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        public ValueTask<string> GetTenantIdentifierAsync()
        {
            var identifier = _httpContextAccessor.HttpContext.Request.Query[MultiTenancyConstants.TenantIdKey];
            return new ValueTask<string>(identifier);
        }
    }
}
