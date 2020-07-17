# TinySaas

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/codeaisy/tinysaas/Build)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/codeaisy.tinysaas)
![Nuget](https://img.shields.io/nuget/dt/CodEaisy.TinySaas)

TinySaas is a C# library for building multitenant applications with .NET Core 3.0+

## Supported Use Cases

- [x] Shared services
- [x] Per-tenant services
- [x] Schema per-tenant (Data Isolation)
- [x] Database per-tenant (Data Isolation)
- [x] Shared database (Data Isolation)
- [x] Shared options
- [x] Per-tenant options
- [x] Shared Authentication and Authorization
- [x] Per-tenant Authentication and Authorization

## Quickstart

- Add dependency to [CodEaisy.TinySaas][nuget_link] from Nuget

```bash
dotnet add package CodEaisy.TinySaas --version 1.0.0-rc5
```

- In `Startup.cs`, add the following inside the `ConfigureServices` method.

  ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        // register all global singleton services here, and also dependencies for your TenantStore and ResolutionStrategy if any

        // ...

        // OPTION 1
        services.AddMultitenancy<Tenant, TenantStore<Tenant>, TenantResolutionStrategy>();

        // OPTION 2
        // uses default `CodEaisy.TinySaas.Model.TinyTenant` as tenant model
        services.AddMultitenancy<TenantStore<TinyTenant>, TenantResolutionStrategy>();

        // ...

        // services.AddControllers();
    }
  ```

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
            .ConfigureMultitenancy<TenantStartup, Tenant>();
            // OPTION 2: add multitenant support via static method
            .ConfigureMultitenancy<Tenant>(ClassName.StaticMethodName);
  ```

  **NOTE**:
  - `Tenant` must implement `CodEaisy.TinySaas.Interface`  `ITenant`.
  - `TenantStore` must implement `CodEaisy.TinySaas.Interface.ITenantStore`.
  - `TenantResolutionStrategy` must implement `CodEaisy.TinySaas.Interface.ITenantResolutionStrategy` respectively.
  - `TenantStartup` must implement `IMultitenantStartup`
  - `ClassName.StaticMethodName` must be of type `System.Action<TTenant, Autofac.ContainerBuilder>` where `TTenant` implements `ITenant`

## Requirements

ASP.NET Core 3.0+

## Changelog

[Learn about the latest improvements][changelog].

## Credits

[Gunnar Peipman](https://gunnarpeipman.com/) and [Michael McKenna](https://michael-mckenna.com/) for their awesome works on Saas in ASP.NET Core.

## Want to help ?

Want to file a bug, contribute some code, or improve documentation? Excellent! Read up on our
guidelines for [contributing][contributing] and then check out one of our issues in the [hotlist: community-help](https://github.com/codeaisy/tinysaas/labels/hotlist%3A%20community-help).

[contributing]: https://github.com/codeaisy/tinysaas/blob/master/CONTRIBUTING.md
[changelog]: https://github.com/angular/angular/blob/master/CHANGELOG.md
[nuget_link]: https://www.nuget.org/packages/CodEaisy.TinySaas
