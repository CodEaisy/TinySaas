using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetTenantContext<T>(this HttpContext context, ITenantContext<T> tenantContext) where T : ITenant
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenantContext, nameof(tenantContext));

            context.Items[MultiTenancyConstants.TenantContextKey] = tenantContext;
        }

        public static ITenantContext<T> GetTenantContext<T>(this HttpContext context)
            where T : ITenant
        {
            Ensure.Argument.NotNull(context, nameof(context));

            context.Items.TryGetValue(MultiTenancyConstants.TenantContextKey, out object tenantContext);

            return tenantContext as ITenantContext<T>;
        }

        public static T GetTenant<T>(this HttpContext context) where T : class, ITenant
        {
            var tenantContext = GetTenantContext<T>(context);

            return tenantContext?.Tenant;
        }

        public static ITenant GetTenant(this HttpContext context)
            => context.GetTenant<ITenant>();
    }
}
