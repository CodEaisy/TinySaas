using System;
using CodEaisy.TinySaas.Interface;

namespace CodEaisy.TinySaas.Model
{
    public class TinyTenant : ITenant
    {
        public Guid Id { get; set; }

        /// <summary>
        /// this should be obtainable from a request via the `ITenantResolutionStrategy`
        /// </summary>
        public string Identifier { get; set; }

        public bool Enabled { get; set; }
    }
}
