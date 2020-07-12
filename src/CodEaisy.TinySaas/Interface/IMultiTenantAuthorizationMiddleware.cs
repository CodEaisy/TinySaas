using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Interface
{
    public interface IMultiTenantAuthorizationMiddleware
    {
        Task Invoke(HttpContext context, IAuthorizationPolicyProvider policyProvider);
    }
}
