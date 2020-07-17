using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Internals;
using CodEaisy.TinySaas.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CodEaisy.TinySaas.Middlewares
{
    internal class MultitenantContainerMiddleware<TTenant> where TTenant : class, ITenant
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MultitenantContainerMiddleware<TTenant>> _logger;

        public MultitenantContainerMiddleware(RequestDelegate next,
            ILogger<MultitenantContainerMiddleware<TTenant>> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context,
            Func<MultitenantContainer<TTenant>> multiTenantContainerAccessor)
        {
            _logger.LogDebug("Setting up container for {tenant}", context.GetCurrentTenant<TTenant>());
            //Set to current tenant container.
            //Begin new scope for request as ASP.NET Core standard scope is per-request
            context.RequestServices =
                new AutofacServiceProvider(multiTenantContainerAccessor()
                        .GetCurrentTenantScope().BeginLifetimeScope());
            await _next.Invoke(context);
        }
    }
}
