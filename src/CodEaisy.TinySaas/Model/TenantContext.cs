using CodEaisy.TinySaas.Interface;

namespace CodEaisy.TinySaas.Model
{
    public class TenantContext<T> : ITenantContext<T> where T : ITenant
    {
        public TenantContext(T tenant)
        {
            Tenant = tenant;
        }

        public T Tenant { get; }
    }
}
