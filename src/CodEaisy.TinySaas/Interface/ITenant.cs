using System;

namespace CodEaisy.TinySaas.Interface
{
    /// <summary>
    /// defining what a tenant looks like
    /// </summary>
    public interface ITenant
    {
        /// <summary>
        /// tenant unique identifier
        /// </summary>
        string Identifier { get; set; }

        /// <summary>
        /// disable tenants, takes tenants offline
        /// </summary>
        bool Enabled { get; }
    }
}
