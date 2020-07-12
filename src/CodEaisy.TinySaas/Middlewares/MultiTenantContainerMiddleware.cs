using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Core;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CodEaisy.TinySaas.Middlewares
{
    internal class MultiTenantContainerMiddleware<TTenant> where TTenant : ITenant
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MultiTenantContainerMiddleware<TTenant>> _logger;

        public MultiTenantContainerMiddleware(RequestDelegate next,
            ILogger<MultiTenantContainerMiddleware<TTenant>> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context,
            Func<MultiTenantContainer<TTenant>> multiTenantContainerAccessor)
        {
            _logger.LogDebug("Setting up container for {tenant}", context.GetTenant());
            //Set to current tenant container.
            //Begin new scope for request as ASP.NET Core standard scope is per-request
            context.RequestServices =
                new AutofacServiceProvider(multiTenantContainerAccessor()
                        .GetCurrentTenantScope().BeginLifetimeScope());
            await _next.Invoke(context);
        }
    }
}
