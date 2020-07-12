using System;
using System.Threading.Tasks;
using CodEaisy.TinySaas.Interface;
using CodEaisy.TinySaas.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodEaisy.TinySaas.Core
{
    public class RedirectMissingTenantHandler : IMissingTenantHandler
    {
        private readonly RequestDelegate _next;
        private readonly RedirectMissingTenantOptions _options;

        public RedirectMissingTenantHandler(RequestDelegate next, RedirectMissingTenantOptions options)
        {
            _next = next;
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext
                .Items
                .ContainsKey(MultiTenancyConstants.TenantContextKey))
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
                return;
            }
        }
    }
}
