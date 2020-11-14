using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Authorization
{
    /// <summary>
    /// authorization middleware result handler
    /// </summary>
    public interface IAuthorizationMiddlewareResultHandler
    {
        /// <summary>
        /// handle authorization result
        /// </summary>
        /// <param name="next"></param>
        /// <param name="context"></param>
        /// <param name="policy"></param>
        /// <param name="authorizeResult"></param>
        Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult);
    }
}
