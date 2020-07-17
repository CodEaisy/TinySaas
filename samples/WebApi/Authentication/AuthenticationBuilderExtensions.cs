using System;
using Microsoft.AspNetCore.Authentication;

namespace CodEaisy.TinySaas.Samples.WebApi.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// adds simple auth
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="options"></param>
        /// <returns>authentication builder</returns>
        public static AuthenticationBuilder AddSimpleAuth(this AuthenticationBuilder authenticationBuilder, Action<SimpleAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<SimpleAuthenticationOptions, SimpleAuthenticationHandler>(SimpleAuthenticationOptions.DefaultScheme, options);
        }
    }
}
