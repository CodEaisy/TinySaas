
namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantContextAccessor<T> where T : ITenant
    {
        T Tenant { get; }
        ITenantContext<T> TenantContext { get; }
    }
}
