using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Extensions
{
    /// <summary>
    /// httpContext extensions
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// set current tenant in HttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenant"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetCurrentTenant<T>(this HttpContext context, T tenant) where T : ITenant
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenant, nameof(tenant));

            context.Items[MultitenancyConstants.TenantContextKey] = tenant;
        }

        /// <summary>
        /// get current tenant from HttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>current tenant</returns>
        public static T GetCurrentTenant<T>(this HttpContext context)
            where T : class, ITenant
        {
            Ensure.Argument.NotNull(context, nameof(context));

            context.Items.TryGetValue(MultitenancyConstants.TenantContextKey, out object tenant);

            return tenant as T;
        }
    }
}
