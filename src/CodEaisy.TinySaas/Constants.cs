namespace CodEaisy.TinySaas
{
    public static class MultiTenancyConstants
    {
        public static string TenantContextKey { get; set; } = "App.Tenant";

        /// <summary>
        /// Has a default value of "AppTenantId"
        /// You can change in Startup.ConfigureServices
        /// </summary>
        public static string TenantIdKey { get; set; } = "AppTenantId";
    }
}
