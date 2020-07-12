using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetCurrentTenant<T>(this HttpContext context, T tenant) where T : ITenant
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenant, nameof(tenant));

            context.Items[MultiTenancyConstants.TenantContextKey] = tenant;
        }

        public static T GetCurrentTenant<T>(this HttpContext context)
            where T : class, ITenant
        {
            Ensure.Argument.NotNull(context, nameof(context));

            context.Items.TryGetValue(MultiTenancyConstants.TenantContextKey, out object tenant);

            return tenant as T;
        }
    }
}
