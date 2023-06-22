using Microsoft.AspNetCore.Mvc;
using Shared.Services;

namespace Shared.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantControllerBase : ControllerBase
    {
        private readonly TenantSingleton _tenantSingleton;

        public TenantControllerBase(TenantSingleton appSingleton) => _tenantSingleton = appSingleton;

        [HttpGet]
        public ActionResult Index() => Ok(_tenantSingleton.GetValue());
    }
}
