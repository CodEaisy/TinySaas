using TinySaasWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace TinySaasWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly TenantSingleton _tenantSingleton;

        public TenantController(TenantSingleton appSingleton) => _tenantSingleton = appSingleton;

        [HttpGet]
        public ActionResult Index() => Ok(_tenantSingleton.GetValue());
    }
}
