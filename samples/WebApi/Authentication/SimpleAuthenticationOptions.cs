using Microsoft.AspNetCore.Authentication;

namespace CodEaisy.TinySaas.Samples.WebApi.Authentication
{
    /// <summary>
    /// api key auth
    /// Credit: https://josef.codes/asp-net-core-protect-your-api-with-api-keys/
    /// </summary>
    public class SimpleAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string AuthHeaderKey = "Authorization";
        public const string DefaultScheme = "ApiKey";
        public string Scheme => DefaultScheme;
        public string AuthenticationType => DefaultScheme;
        public string AuthSecret { get; set; }
    }
}
