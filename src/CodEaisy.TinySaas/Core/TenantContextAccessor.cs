using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Core
{
    public class TenantContextAccessor<T>: ITenantContextAccessor<T> where T : class, ITenant
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T Tenant => _httpContextAccessor.HttpContext.GetTenant<T>();

        public ITenantContext<T> TenantContext => _httpContextAccessor.HttpContext.GetTenantContext<T>();
    }
}
