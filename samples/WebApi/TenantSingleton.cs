using System;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    public class TenantSingleton
    {
        private readonly Guid _testValue;

        public TenantSingleton ()
        {
            _testValue = Guid.NewGuid();
        }

        public Guid GetTestValue()
        {
            return _testValue;
        }
    }
}
