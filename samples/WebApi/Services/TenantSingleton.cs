using System;

namespace CodEaisy.TinySaas.Samples.WebApi.Services
{
    public class TenantSingleton
    {
        private readonly Guid _testValue;

        public TenantSingleton ()
        {
            _testValue = Guid.NewGuid();
        }

        public Guid GetValue()
        {
            return _testValue;
        }
    }
}
