using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Extensions;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Internals
{
    /// <summary>
    /// default tenant accessor implementation
    /// </summary>
    /// <typeparam name="T">type of tenant</typeparam>
    public class TenantAccessor<T>: ITenantAccessor<T> where T : class, ITenant
    {
        /// <summary>
        /// http context accessor
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public TenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// get current tenant from <type cref="HttpContext"/>
        /// </summary>
        /// <returns>tenant instance</returns>
        public T Tenant => _httpContextAccessor.HttpContext.GetCurrentTenant<T>();
    }
}
