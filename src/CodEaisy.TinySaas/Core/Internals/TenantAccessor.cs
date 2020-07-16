using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Core.Internals
{
    internal class TenantAccessor<T>: ITenantAccessor<T> where T : class, ITenant
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T Tenant => _httpContextAccessor.HttpContext.GetCurrentTenant<T>();
    }
}
