using CodEaisy.TinySaas.Core;
using CodEaisy.TinySaas.Interface;
using CodEaisy.TinySaas.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CodEaisy.TinySaas
{
    public static class ApplicationBuilderExtensions
    {
        #region individual middleware registrations

        /// <summary>
        /// add multitenant container
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IApplicationBuilder UseMultiTenantContainer<TTenant>(this IApplicationBuilder app) where TTenant : class, ITenant
            => app.UseMiddleware<MultiTenantContainerMiddleware<TTenant>>();

        /// <summary>
        /// add multitenancy resolution middleware and a missing tenant handler
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TTenant"></typeparam>
        private static IApplicationBuilder UseTenantResolutionMiddleware<TTenant>(this IApplicationBuilder app)
            where TTenant : ITenant
        {
            app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();
            return app;
        }

        /// <summary>
        /// add multitenancy with default resolution middleware and a missing tenant handler
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TMissingTenantHandler"></typeparam>
        private static IApplicationBuilder UseMissingTenantHandler<TMissingTenantHandler, TMissingTenantOptions>(this IApplicationBuilder app, TMissingTenantOptions options)
            where TMissingTenantHandler : IMissingTenantHandler
            where TMissingTenantOptions : IMissingTenantOptions
        {
            app.UseMiddleware<TMissingTenantHandler>(options);
            return app;
        }

        /// <summary>
        /// add multitenancy with default resolution middleware and a missing tenant handler
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TMissingTenantHandler"></typeparam>
        private static IApplicationBuilder UseMissingTenantHandler<TMissingTenantHandler>(this IApplicationBuilder app)
            where TMissingTenantHandler : IMissingTenantHandler
        {
            app.UseMiddleware<TMissingTenantHandler>();
            return app;
        }

        #endregion

        #region UseMultitenancy

        /// <summary>
        /// add multitenancy with default resolution middleware with missing tenant handler
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TTenant"></typeparam>
        /// <typeparam name="TMissingTenantHandler"></typeparam>
        public static IApplicationBuilder UseMultitenancy<TTenant, TMissingTenantHandler>(this IApplicationBuilder app)
            where TTenant : class, ITenant
            where TMissingTenantHandler : IMissingTenantHandler
        {
            app.UseMultiTenantContainer<TTenant>()
                .UseTenantResolutionMiddleware<TTenant>()
                .UseMissingTenantHandler<TMissingTenantHandler>();

            return app;
        }

        /// <summary>
        /// add multitenancy with default resolution middleware with missing tenant handler with constructor arguments
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <typeparam name="TTenant"></typeparam>
        /// <typeparam name="TMissingTenantHandler"></typeparam>
        /// <typeparam name="TMissingTenantOptions"></typeparam>
        public static IApplicationBuilder UseMultitenancy<TTenant, TMissingTenantHandler, TMissingTenantOptions>(this IApplicationBuilder app, TMissingTenantOptions options)
            where TTenant : class, ITenant
            where TMissingTenantHandler : IMissingTenantHandler
            where TMissingTenantOptions : IMissingTenantOptions
        {
            app.UseMultiTenantContainer<TTenant>()
                .UseTenantResolutionMiddleware<TTenant>()
                .UseMissingTenantHandler<TMissingTenantHandler, TMissingTenantOptions>(options);

            return app;
        }

        #endregion

        /// <summary>
        /// Use the Tenant Auth to process the authentication handlers
        /// </summary>
        /// <param name="builder"></param>
        public static IApplicationBuilder UseMultiTenantAuthentication(this IApplicationBuilder builder)
            => builder.UseMiddleware<MultiTenantAuthenticationMiddleware>();

        /// <summary>
        /// Use the Tenant Auth to process the authorization handlers
        /// </summary>
        /// <param name="builder"></param>
        public static IApplicationBuilder UseMultiTenantAuthorization<T>(this IApplicationBuilder builder)
            where T: IMultiTenantAuthorizationMiddleware
            => builder.UseMiddleware<T>();
    }
}
