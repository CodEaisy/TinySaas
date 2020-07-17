using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Samples.WebApi.Authentication
{
    /// <summary>
    /// api key auth handler (subject to change)
    /// </summary>
    public class SimpleAuthenticationHandler : AuthenticationHandler<SimpleAuthenticationOptions>
    {
        public SimpleAuthenticationHandler(
            IOptionsMonitor<SimpleAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// handle simple auth authentication requests
        /// </summary>
        /// <returns>authentication result</returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var hasApiKey = Request.Headers.TryGetValue(SimpleAuthenticationOptions.AuthHeaderKey,
                out var apiKeyHeaderValues);

            if (!hasApiKey)
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (providedApiKey == Options.AuthSecret)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Actor, "Known User"),
                };

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return AuthenticateResult.Fail("Unknown key provided");
        }

        /// <summary>
        /// handle authorization challenge
        /// </summary>
        /// <param name="properties"></param>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;

            await Response.WriteAsync("Unauthorized");
        }

        /// <summary>
        /// handle forbidden challenge
        /// </summary>
        /// <param name="properties"></param>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;

            await Response.WriteAsync(JsonSerializer.Serialize("Forbidden"));
        }
    }
}
