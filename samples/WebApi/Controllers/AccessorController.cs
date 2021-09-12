using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessorController : ControllerBase
    {
        private readonly ITenantAccessor<SimpleTenant> _accessor;

        public AccessorController(ITenantAccessor<SimpleTenant> accessor) => _accessor = accessor;

        [HttpGet]
        public ActionResult Index() => Ok(_accessor.Tenant.Name);
    }
}
