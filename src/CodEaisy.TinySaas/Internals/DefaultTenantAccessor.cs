using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Internals
{
    /// <summary>
    /// Default implementation of the tenant accessor
    /// </summary>
    public class DefaultTenantAccessor : ITenantAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// initialize an instance of the DefaultTenantAccessor
        /// </summary>
        public DefaultTenantAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// Get current tenant
        /// </summary>
        public ITenant GetTenant()
        {
            return _accessor.HttpContext.GetCurrentTenant<ITenant>();
        }

        T ITenantAccessor.GetTenant<T>()
        {
            return GetTenant() as T;
        }
    }
}
