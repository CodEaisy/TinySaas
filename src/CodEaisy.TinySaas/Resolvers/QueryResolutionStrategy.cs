using System.Threading.Tasks;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Resolvers
{
    /// <summary>
    /// <para>Resolve a query parameter to a tenant identifier.</para>
    /// <para>Uses CodEaisy.TinySaas.MultitenancyConstants.TenantIdKey as key</para>
    /// </summary>
    public class QueryResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public QueryResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <returns>tenant identifier</returns>
        public ValueTask<string> GetTenantIdentifierAsync()
        {
            var identifier = _httpContextAccessor.HttpContext.Request.Query[MultitenancyConstants.TenantIdKey];
            return new ValueTask<string>(identifier);
        }
    }
}
