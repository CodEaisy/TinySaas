using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CodEaisy.TinySaas.Extensions
{
    /// <summary>
    /// appbuilder extensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// add multitenant container
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TTenant"></typeparam>
        internal static IApplicationBuilder UseMultitenantContainer<TTenant>(this IApplicationBuilder app)
            where TTenant : class, ITenant
            => app.UseMiddleware<MultitenantContainerMiddleware<TTenant>>()
                .UseTenantResolutionMiddleware<TTenant>();

        /// <summary>
        /// adds tenant resolution middleware
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TTenant"></typeparam>
        internal static IApplicationBuilder UseTenantResolutionMiddleware<TTenant>(this IApplicationBuilder app)
            where TTenant : ITenant
            => app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();

        #region UseMultitenancy

        /// <summary>
        /// add multitenancy with custom tenant model
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TTenant"></typeparam>
        public static IApplicationBuilder UseMultitenancy<TTenant>(this IApplicationBuilder app)
            where TTenant : class, ITenant
            => app.UseMultitenantContainer<TTenant>();

        #endregion
    }
}
