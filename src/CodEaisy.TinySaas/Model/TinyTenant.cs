using CodEaisy.TinySaas.Interface;

namespace CodEaisy.TinySaas.Model
{
    public class TinyTenant : ITenant
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public bool Enabled { get; set; }
    }
}
