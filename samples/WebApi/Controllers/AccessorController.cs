using CodEaisy.TinySaas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessorController : ControllerBase
    {
        private readonly SimpleTenant _tenant;

        public AccessorController(SimpleTenant tenant) => _tenant = tenant;

        [HttpGet]
        public ActionResult Index() => Ok(_tenant.Name);
    }
}
