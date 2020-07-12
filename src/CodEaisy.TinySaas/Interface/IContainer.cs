using System;
using Autofac;

namespace CodEaisy.TinySaas.Interface
{
    public interface ICustomContainer : IDisposable
    {
        ILifetimeScope GetCurrentTenantScope();
        ILifetimeScope GetTenantScope(string tenantId);
    }
}
