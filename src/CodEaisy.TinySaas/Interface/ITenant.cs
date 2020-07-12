using System;

namespace CodEaisy.TinySaas.Interface
{
    /// <summary>
    /// defining what a tenant looks like
    /// </summary>
    public interface ITenant
    {
        /// <summary>
        /// tenant identifier
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// tenant known name, will be used as page title
        /// </summary>
        string Name { get; }

        /// <summary>
        /// tenant url (without the scheme), subdomain works
        /// </summary>
        string Host { get; }

        /// <summary>
        /// disable tenants, takes tenants offline
        /// </summary>
        bool Enabled { get; set; }
    }
}
