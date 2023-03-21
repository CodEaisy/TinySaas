using System.Threading.Tasks;
using CodEaisy.TinySaas.Extensions;
using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodEaisy.TinySaas.Middlewares
{
    /// <summary>
    /// tenant resolution middleware
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TenantResolutionMiddleware<T> where T : class, ITenant
    {
        /// <summary>
        /// request delegate
        /// </summary>
        protected readonly RequestDelegate _next;

        /// <summary>
        /// logger
        /// </summary>
        protected readonly ILogger<TenantResolutionMiddleware<T>> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILogger<TenantResolutionMiddleware<T>> logger)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(logger, nameof(logger));

            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// try to resolve tenant, add it to <see cref="HttpContext"/> when available
        /// </summary>
        /// <param name="context"></param>
        public virtual async Task Invoke(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            if (!context.Items.ContainsKey(MultitenancyConstants.TenantContextKey))
            {
                var tenantService = context.RequestServices.GetRequiredService<ITenantService<T>>();
                _logger.LogDebug("Resolving TenantContext using {loggerType}.", tenantService.GetType().Name);

                var tenant = await tenantService.GetTenant();

                if (tenant != null && tenant.Enabled)
                {
                    _logger.LogDebug("TenantContext Resolved. Adding to HttpContext.");
                    context.SetCurrentTenant(tenant);
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
