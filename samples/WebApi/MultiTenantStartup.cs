using System;
using System.Security.Claims;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Samples.WebApi.Authentication;
using CodEaisy.TinySaas.Samples.WebApi.Authorization;
using CodEaisy.TinySaas.Samples.WebApi.Options;
using CodEaisy.TinySaas.Samples.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodEaisy.TinySaas.Samples.WebApi
{
    public class MultitenantStartup : IMultitenantStartup<SimpleTenant>
    {
        public void ConfigureServices(SimpleTenant tenant, ContainerBuilder container)
        {
            #region inbuilt DI pattern
            // to register services, you can either use inbuilt DI pattern
            // 1. create a service collection
            var services = new ServiceCollection();

            // 2. register services to your service collection
            // tenant singleton, registers a single instance for each tenant
            services.AddSingleton<TenantSingleton>();

            services.AddOptions<TenantOption>()
                .Configure(options => {
                    options.Value = Guid.NewGuid();
                });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = SimpleAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = SimpleAuthenticationOptions.DefaultScheme;
            }).AddSimpleAuth(options => options.AuthSecret = tenant.AuthSecret);

            services.AddAuthorizationCore(options => {
                options.AddPolicy(Policies.SimpleAuth, policy => {
                    policy.RequireClaim(ClaimTypes.Actor);
                });
            });

            // 3. add the services into the container
            container.Populate(services);
            #endregion
        }
    }
}
