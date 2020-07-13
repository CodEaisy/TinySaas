# TinySaas ![Build](https://github.com/CodEaisy/TinySaas/workflows/Build/badge.svg)

TinySaas is a C# library for building multitenant applications with .NET Core 3.0+

## Supported Use Cases

- [x] Per-Tenant Services
- [x] Shared Tenant Services
- [x] Schema per Tenant (Data Isolation)
- [x] Database per Tenant (Data Isolation)
- [x] Shared Database (Data Isolation)

## Requirements

ASP.NET Core 3.0+

## How to

- In `Startup.cs`, add the following inside the `ConfigureServices` method.

  ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        // register all global singleton services here, and also dependencies for your TenantStore and ResolutionStrategy if any

        // ...

        // OPTION 1
        services.AddMultiTenancy<Tenant, TenantStore<Tenant>, ResolutionStrategy>();

        // OPTION 2
        // uses default `CodEaisy.TinySaas.Model.TinyTenant` as tenant model
        services.AddMultiTenancy<TenantStore<TinyTenant>, TenantResolutionStrategy>();

        // ...

        // services.AddControllers();
    }
  ```

  NB: Option 1 - `Tenant` must implement `CodEaisy.TinySaas.Interface.ITenant`
  `TenantStore` and `TenantResolutionStrategy` must implement `CodEaisy.TinySaas.Interface.ITenantStore` and `CodEaisy.TinySaas.Interface.ITenantResolutionStrategy` respectively.

- In `Program.cs`, add the following in the `CreateHostBuilder` method.

  ```csharp
  public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            // OPTION 1: add multitenant support via TenantStartup class
            .ConfigureMultiTenancy<TenantStartup, Tenant>();
            // OPTION 2: add multitenant support via static method
            .ConfigureMultiTenancy<Tenant>(ClassName.StaticMethodName);
  ```

  NB: `TenantStartup` must implement `IMultiTenantStartup`
  `ClassName.StaticMethodName` must be of type `System.Action<TTenant, Autofac.ContainerBuilder>` where `TTenant` implements `ITenant`
