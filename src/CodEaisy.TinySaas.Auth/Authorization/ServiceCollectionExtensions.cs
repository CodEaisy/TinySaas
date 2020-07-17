using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Authorization
{
    /// <summary>
    /// service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// register <see cref="IAuthorizationMiddlewareResultHandler"/> needed for <see cref="MultitenantAuthorizationMiddleware"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPerTenantAuthorization(this IServiceCollection services)
            => services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();
    }
}
