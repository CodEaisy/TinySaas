# TinySaas

![Build](https://github.com/CodEaisy/TinySaas/workflows/Build/badge.svg)

TinySaas is a C# library for building multitenant applications with .NET Core 3.0+

## Supported Use Cases

- [x] Per-Tenant Services
- [x] Shared Tenant Services
- [x] Schema per Tenant (Data Isolation)
- [x] Database per Tenant (Data Isolation)
- [x] Shared Database (Data Isolation)

## Quickstart

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

  Then, add the following in the `Configure` method

  ```csharp
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
      if (env.IsDevelopment())
      {
          app.UseDeveloperExceptionPage();
      }

      // enable multitenant support, with missing tenant handler and tenant container

      // OPTION 1
      // missing tenant handler has a dependency that can be provided immediately
      app.UseMultitenancy<Tenant, MissingTenantHandler, MissingTenantOptions>(missingTenantOptions);

      // OPTION 2
      // missing tenant handler does not have a dependency or dependency is already registered in services
      app.UseMultitenancy<Tenant, MissingTenantHandler>();

      // OPTION 3
      // Use `SimpleTenant` as tenant model, and missing tenant handler does not have a dependency or dependency is already registered in services
      app.UseMultitenancy<TMissingTenantHandler>()

      // ...
  }
  ```

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

## Requirements

ASP.NET Core 3.0+

## Changelog

[Learn about the latest improvements][changelog].

## Want to help ?

Want to file a bug, contribute some code, or improve documentation? Excellent! Read up on our
guidelines for [contributing][contributing] and then check out one of our issues in the [hotlist: community-help](https://github.com/codeaisy/tinysaas/labels/hotlist%3A%20community-help).

[contributing]: https://github.com/codeaisy/tinysaas/blob/master/CONTRIBUTING.md
[changelog]: https://github.com/angular/angular/blob/master/CHANGELOG.md
