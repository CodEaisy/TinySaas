using System;

namespace DefaultWebApi.Services
{
    public class AppSingleton
    {
        private readonly Guid _value;

        public AppSingleton()
        {
            _value = Guid.NewGuid();
        }

        public Guid GetValue() => _value;
    }
}
