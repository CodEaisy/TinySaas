using System;
using Autofac;
using CodEaisy.TinySaas.Core;
using CodEaisy.TinySaas.Interface;
using CodEaisy.TinySaas.Model;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Extensions
{
    public static class OptionsExtension
    {
         /// <summary>
        /// Register tenant specific options
        /// </summary>
        /// <typeparam name="TOptions">Type of options we are apply configuration to</typeparam>
        /// <param name="tenantConfig">Action to configure options for a tenant</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterTenantOptions<TOptions, T>(this ContainerBuilder builder, Action<TOptions, T> tenantConfig) where TOptions : class, new() where T : ITenant
        {
            builder.RegisterType<TenantOptionsCache<TOptions, T>>()
                .As<IOptionsMonitorCache<TOptions>>()
                .SingleInstance();

            builder.RegisterType<TenantOptionsFactory<TOptions, T>>()
                .As<IOptionsFactory<TOptions>>()
                .WithParameter(new TypedParameter(typeof(Action<TOptions, T>), tenantConfig))
                .SingleInstance();

            builder.RegisterType<TenantOptions<TOptions>>()
                .As<IOptionsSnapshot<TOptions>>()
                .SingleInstance();

            builder.RegisterType<TenantOptions<TOptions>>()
                .As<IOptions<TOptions>>()
                .SingleInstance();

            return builder;
        }
    }
}
