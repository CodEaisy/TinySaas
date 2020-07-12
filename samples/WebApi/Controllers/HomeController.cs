using System;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly TenantSingleton _singleton;

        public HomeController(TenantSingleton singleton)
        {
            _singleton = singleton;
        }

        [HttpGet("singleton")]
        public Guid TestTenantSingleton()
        {
            return _singleton.GetTestValue();
        }
    }
}
