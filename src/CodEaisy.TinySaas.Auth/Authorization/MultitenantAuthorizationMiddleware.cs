// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Authorization
{
    /// <summary>
    /// multitenant authorization handler
    /// </summary>
    public abstract class MultitenantAuthorizationMiddleware
    {
        // AppContext switch used to control whether HttpContext or endpoint is passed as a resource to AuthZ
        private const string SuppressUseHttpContextAsAuthorizationResource = "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource";

        // Property key is used by Endpoint routing to determine if Authorization has run
        private const string AuthorizationMiddlewareInvokedWithEndpointKey = "__AuthorizationMiddlewareWithEndpointInvoked";
        private static readonly object AuthorizationMiddlewareWithEndpointInvokedValue = new object();

        private readonly RequestDelegate _next;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        protected MultitenantAuthorizationMiddleware(RequestDelegate next)
        {
            Ensure.Argument.NotNull(next);
            _next = next;
        }

        /// <summary>
        /// handle per tenant authorization
        /// </summary>
        /// <param name="context"></param>
        /// <param name="policyProvider"></param>
        public async Task Invoke(HttpContext context, IAuthorizationPolicyProvider policyProvider)
        {
            Ensure.Argument.NotNull(context);
            Ensure.Argument.NotNull(policyProvider);

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var policyProvider_ = policyProvider;

            if (endpoint != null)
            {
                // EndpointRoutingMiddleware uses this flag to check if the Authorization middleware processed auth metadata on the endpoint.
                // The Authorization middleware can only make this claim if it observes an actual endpoint.
                context.Items[AuthorizationMiddlewareInvokedWithEndpointKey] = AuthorizationMiddlewareWithEndpointInvokedValue;
            }

            // IMPORTANT: Changes to authorization logic should be mirrored in MVC's AuthorizeFilter
            var authorizeData = GetAuthorizeData(endpoint);
            var policy = await AuthorizationPolicy.CombineAsync(policyProvider_, authorizeData);
            if (policy == null)
            {
                await _next(context);
                return;
            }

            // Policy evaluator has transient lifetime so it fetched from request services instead of injecting in constructor
            var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();

            var authenticateResult = await policyEvaluator.AuthenticateAsync(policy, context);

            // Allow Anonymous skips all authorization
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            object resource;
            if (AppContext.TryGetSwitch(SuppressUseHttpContextAsAuthorizationResource, out var useEndpointAsResource) && useEndpointAsResource)
            {
                resource = endpoint;
            }
            else
            {
                resource = context;
            }

            var authorizeResult = await policyEvaluator.AuthorizeAsync(policy, authenticateResult, context, resource);
            var authorizationMiddlewareResultHandler = context.RequestServices.GetRequiredService<IAuthorizationMiddlewareResultHandler>();
            await authorizationMiddlewareResultHandler.HandleAsync(_next, context, policy, authorizeResult);
        }

        /// <summary>
        /// get authorize data
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns><see cref="T:IReadOnlyList:IAuthorizeData"/></returns>
        protected abstract IReadOnlyList<IAuthorizeData> GetAuthorizeData(Endpoint endpoint);
    }
}
