using System;

namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantContext<T> where T: ITenant
    {
        public Guid Id { get => Tenant.Id; }
        public T Tenant { get; }
    }
}
