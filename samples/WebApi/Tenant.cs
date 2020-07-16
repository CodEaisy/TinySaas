using System;
using CodEaisy.TinySaas.Interface;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    public class Tenant : ITenant
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public string Host { get; set; }

        public bool Enabled { get; set; }
    }
}
