using System.Threading.Tasks;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodEaisy.TinySaas.Core
{
    public class TenantResolutionMiddleware<T> where T : ITenant
    {
        protected readonly RequestDelegate _next;
        protected readonly ILogger<TenantResolutionMiddleware<T>> _logger;

        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILogger<TenantResolutionMiddleware<T>> logger)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(logger, nameof(logger));

            _next = next;
            _logger = logger;
        }

        public virtual async Task Invoke(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            if (!context.Items.ContainsKey(MultiTenancyConstants.TenantContextKey))
            {
                var tenantContextService = context.RequestServices.GetRequiredService<ITenantService<T>>();
                _logger.LogDebug("Resolving TenantContext using {loggerType}.", tenantContextService.GetType().Name);

                var tenantContext = await tenantContextService.GetTenant();

                if (tenantContext != null)
                {
                    _logger.LogDebug("TenantContext Resolved. Adding to HttpContext.");
                    context.SetTenantContext(tenantContext);
                }
                else
                {
                    _logger.LogDebug("TenantContext Not Resolved.");
                }
            }

             //Continue processing
            if (_next != null)
                await _next(context);
        }
    }
}
