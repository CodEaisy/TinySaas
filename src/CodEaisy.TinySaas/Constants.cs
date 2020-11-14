namespace CodEaisy.TinySaas
{
    /// <summary>
    /// common multitenancy helper constants
    /// </summary>
    public static class MultitenancyConstants
    {
        /// <summary>
        /// key for tenant context information in HttpContext.Items
        /// </summary>
        public const string TenantContextKey = "TINYSAAS.TENANTCONTEXT.KEY";

        /// <summary>
        /// key to be used when using header or query resolution strategies
        /// change in Startup.ConfigureService if need be
        /// </summary>
        public static string TenantIdKey { get; set; } = "TinySaasTenantId";
    }
}
