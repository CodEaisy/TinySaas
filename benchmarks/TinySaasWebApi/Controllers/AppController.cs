using Shared.Services;

namespace Shared.Controllers
{
    public class AppController : AppControllerBase
    {
        public AppController(AppSingleton appSingleton): base(appSingleton) { }
    }
}
