using CodEaisy.TinySaas.Samples.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = Policies.SimpleAuth)]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index() => Ok("Authenticated");
    }
}
