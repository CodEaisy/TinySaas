
namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantAccessor<T> where T : ITenant
    {
        T Tenant { get; }
    }
}
