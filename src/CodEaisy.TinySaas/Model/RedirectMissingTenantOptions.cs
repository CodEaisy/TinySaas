using CodEaisy.TinySaas.Interface;

namespace CodEaisy.TinySaas.Model
{
    public class RedirectMissingTenantOptions : IMissingTenantOptions
    {
        public string RedirectUrl { get; set; }
    }
}
