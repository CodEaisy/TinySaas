using CodEaisy.TinySaas.Authentication;
using Microsoft.AspNetCore.Builder;

namespace CodEaisy.TinySaas.Extensions
{
    /// <summary>
    /// authentication middleware extensions
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use the Tenant Auth to process the authentication handlers
        /// </summary>
        /// <param name="builder"></param>
        public static IApplicationBuilder UsePerTenantAuthentication(this IApplicationBuilder builder)
            => builder.UseMiddleware<MultitenantAuthenticationMiddleware>();
    }
}
