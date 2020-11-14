using System;
using CodEaisy.TinySaas.Interfaces;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    public class SimpleTenant : ITenant
    {
        /// <summary>
        /// tenant primary key
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// this should be obtainable from a request via the `ITenantResolutionStrategy`
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// given name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// host url
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// indicate whether a tenant is enabled or otherwise
        /// </summary>
        public bool Enabled { get; set; }

        public string AuthSecret { get; set; }
    }
}
