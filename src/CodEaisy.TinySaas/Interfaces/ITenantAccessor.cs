
namespace CodEaisy.TinySaas.Interfaces
{
    /// <summary>
    /// access current running tenant
    /// </summary>
    public interface ITenantAccessor
    {
        /// <summary>
        /// get tenant for the current request
        /// </summary>
        ITenant GetTenant();

        /// <summary>
        /// get tenant for the current request
        /// </summary>
        T GetTenant<T>() where T: class, ITenant;
    }
}
