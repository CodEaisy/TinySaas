using CodEaisy.TinySaas.Authorization;
using Microsoft.AspNetCore.Builder;

namespace CodEaisy.TinySaas.Extensions
{
    /// <summary>
    /// multitenant authorization extension
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use the Tenant Auth to process the authorization handlers
        /// </summary>
        /// <param name="builder"></param>
        public static IApplicationBuilder UsePerTenantAuthorization<T>(this IApplicationBuilder builder) where T : MultitenantAuthorizationMiddleware
            => builder.UseMiddleware<T>();
    }
}
