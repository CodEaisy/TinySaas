using Autofac;

namespace CodEaisy.TinySaas.Interface
{
    public interface IMultiTenantStartup<TTenant> where TTenant : ITenant
    {
        void ConfigureServices(ITenantContext<TTenant> context, ContainerBuilder container);
    }
}
