using CodEaisy.TinySaas.Samples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
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
