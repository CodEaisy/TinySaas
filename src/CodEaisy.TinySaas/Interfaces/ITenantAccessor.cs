
namespace CodEaisy.TinySaas.Interfaces
{
    /// <summary>
    /// wrapping a tenant in an accessor, so you can add more info later
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenantAccessor<out T> where T : ITenant
    {
        /// <summary>
        /// tenant
        /// </summary>
        T Tenant { get; }
    }
}
