using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessorController : ControllerBase
    {
        private readonly ITenantAccessor _accessor;

        public AccessorController(ITenantAccessor accessor) => _accessor = accessor;

        [HttpGet]
        public ActionResult Index() => Ok(_accessor.GetTenant<SimpleTenant>().Name);
    }
}
