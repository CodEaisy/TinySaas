using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodEaisy.TinySaas.MissingTenants
{
    /// <summary>
    /// redirect missing tenant request to specified uri
    /// </summary>
    public class RedirectMissingTenantHandler : IMissingTenantHandler
    {
        private readonly RequestDelegate _next;
        private readonly RedirectMissingTenantOptions _options;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="optionsBuilder"></param>
        public RedirectMissingTenantHandler(RequestDelegate next, Action<RedirectMissingTenantOptions> optionsBuilder)
        {
            _next = next;
            if (optionsBuilder != null)
            {
                _options = new RedirectMissingTenantOptions();
                optionsBuilder.Invoke(_options);
            }
        }

        /// <summary>
        /// redirect missing tenant to specified url
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext
                .Items
                .ContainsKey(MultitenancyConstants.TenantContextKey))
            {
                await _next.Invoke(httpContext).ConfigureAwait(false);
            }
            else
            {
                var logger = httpContext.RequestServices.GetRequiredService<ILogger<RedirectMissingTenantHandler>>();
                Ensure.NotNull(_options);

                var redirectUrl = $"{_options.RedirectUrl}{httpContext.Request.Path.Value}{httpContext.Request.QueryString.ToUriComponent()}";

                logger.LogInformation("Redirecting to default tenant {options}", _options);
                httpContext.Response.Redirect(redirectUrl);
            }
        }
    }
}
