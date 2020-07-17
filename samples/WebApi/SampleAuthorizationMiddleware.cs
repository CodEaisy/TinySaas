using System;
using System.Collections.Generic;
using CodEaisy.TinySaas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    /// <summary>
    /// sample authorization middleware
    /// </summary>
    public class SampleAuthorizationMiddleware : MultitenantAuthorizationMiddleware
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        public SampleAuthorizationMiddleware(RequestDelegate next) : base(next)
        {
        }

        protected override IReadOnlyList<IAuthorizeData> GetAuthorizeData(Endpoint endpoint) =>
            endpoint?.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();
    }
}
