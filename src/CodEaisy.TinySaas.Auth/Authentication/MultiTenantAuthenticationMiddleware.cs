using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Authentication
{
    /// <summary>
    /// AuthenticationMiddleware.cs from framework with injection point moved
    /// </summary>
    internal class MultitenantAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public MultitenantAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context, IAuthenticationSchemeProvider Schemes)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });

            // Give any IAuthenticationRequestHandler schemes a chance to handle the request
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync().ConfigureAwait(false))
            {
                if (await handlers.GetHandlerAsync(context, scheme.Name).ConfigureAwait(false) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync().ConfigureAwait(false))
                {
                    return;
                }
            }

            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                {
                    context.User = result.Principal;
                }
            }

            await _next(context);
        }
    }
}
