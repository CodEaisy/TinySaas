using System;
using Microsoft.AspNetCore.Builder;

namespace CodEaisy.TinySaas.MissingTenants
{
    /// <summary>
    /// application builder extensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// adds missing tenant handler with options
        /// </summary>
        /// <param name="app"></param>
        /// <param name="optionsBuilder"></param>
        /// <typeparam name="TMissingTenantHandler"></typeparam>
        /// <typeparam name="TMissingTenantOptions"></typeparam>
        public static IApplicationBuilder UseMissingTenantHandler<TMissingTenantHandler, TMissingTenantOptions>(this IApplicationBuilder app, Action<TMissingTenantOptions> optionsBuilder)
            where TMissingTenantHandler : class
            where TMissingTenantOptions : class
        {
            app.UseMiddleware<TMissingTenantHandler>(optionsBuilder);
            return app;
        }

        /// <summary>
        /// adds a missing tenant handler
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TMissingTenantHandler"></typeparam>
        public static IApplicationBuilder UseMissingTenantHandler<TMissingTenantHandler>(this IApplicationBuilder app)
            where TMissingTenantHandler : IMissingTenantHandler
        {
            app.UseMiddleware<TMissingTenantHandler>();
            return app;
        }
    }
}
