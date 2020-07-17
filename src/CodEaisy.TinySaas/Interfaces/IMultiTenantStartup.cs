using Autofac;

namespace CodEaisy.TinySaas.Interfaces
{
    /// <summary>
    /// multi-tenant startup interface
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public interface IMultitenantStartup<in TTenant> where TTenant : ITenant
    {
        /// <summary>
        /// configure multi-tenant services
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="container"></param>
        void ConfigureServices(TTenant tenant, ContainerBuilder container);
    }
}
