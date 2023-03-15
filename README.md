# TinySaas

[![Build][build_badge]][build_link] &ensp;
[![Coverage][coverage_badge]][coverage_link] &ensp;
[![Nuget (with prereleases)][nuget_badge]][nuget_link]

TinySaas is a C# library for building multitenant applications with .NET 6.0+, version 1.0 supports .NET Core 3.1+

## Supported Use Cases

- [X] Shared services
- [X] Per-tenant services
- [X] Schema per-tenant (Data Isolation)
- [X] Database per-tenant (Data Isolation)
- [X] Shared database (Data Isolation)
- [X] Shared options
- [X] Per-tenant options
- [X] Shared Authentication and Authorization
- [X] Per-tenant Authentication and Authorization

## Quickstart

- Add dependency to [CodEaisy.TinySaas][nuget_link] from Nuget

```bash
dotnet add package CodEaisy.TinySaas.AspNetCore --version 2.0.0
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

  If using minimal APIs, you can add multitenany support by doing the following:

  ```csharp
  // added multitenancy to Host builder
  // OPTION 1: add multitenant support via TenantStartup class
  builder.Host.ConfigureMultitenancy<TenantStartup, Tenant>();
  // OPTION 2: add multitenant support via static method
  builder.Host.ConfigureMultitenancy<Tenant>(ClassName.StaticMethodName);
  ```

  **NOTE**:

  - `Tenant` must implement `CodEaisy.TinySaas.Interface`  `ITenant`.
  - `TenantStore` must implement `CodEaisy.TinySaas.Interface.ITenantStore`.
  - `TenantResolutionStrategy` must implement `CodEaisy.TinySaas.Interface.ITenantResolutionStrategy` respectively.
  - `TenantStartup` must implement `IMultitenantStartup`
  - `ClassName.StaticMethodName` must be of type `System.Action<TTenant, Autofac.ContainerBuilder>` where `TTenant` implements `ITenant`

## Benchmarks

Here, we show the performance report of an application singleton in a default ASP.NET application and an application singleton in a TinySaas ASP.NET application.

``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.2 (22D49) [Darwin 22.3.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK=7.0.102
  [Host]   : .NET 6.0.14 (6.0.1423.7309), Arm64 RyuJIT AdvSIMD
  .NET 6.0 : .NET 6.0.14 (6.0.1423.7309), Arm64 RyuJIT AdvSIMD
  .NET 7.0 : .NET 7.0.2 (7.0.222.60605), Arm64 RyuJIT AdvSIMD

```

### App Singleton in Default ASP.NET vs TinySaas vs OrchardCore

|      Method |      Job |  Runtime |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------ |--------- |--------- |---------:|---------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
|     Default | .NET 6.0 | .NET 6.0 | 49.50 μs | 1.541 μs | 4.470 μs |  1.00 |    0.00 |  5.1270 |      - |  10.65 KB |        1.00 |
| OrchardCore | .NET 6.0 | .NET 6.0 | 65.73 μs | 1.300 μs | 3.514 μs |  1.35 |    0.15 |  8.7891 |      - |  17.85 KB |        1.68 |
|    TinySaas | .NET 6.0 | .NET 6.0 | 55.80 μs | 1.109 μs | 1.853 μs |  1.14 |    0.10 | 10.2539 |      - |  20.83 KB |        1.96 |
|             |          |          |          |          |          |       |         |         |        |           |             |
|     Default | .NET 7.0 | .NET 7.0 | 32.15 μs | 0.686 μs | 1.923 μs |  1.00 |    0.00 |  1.7090 |      - |  10.45 KB |        1.00 |
| OrchardCore | .NET 7.0 | .NET 7.0 | 60.42 μs | 2.173 μs | 6.338 μs |  1.89 |    0.24 |  2.6855 |      - |  17.38 KB |        1.66 |
|    TinySaas | .NET 7.0 | .NET 7.0 | 52.87 μs | 1.132 μs | 3.231 μs |  1.65 |    0.15 |  3.2959 | 0.1221 |  20.63 KB |        1.97 |

### Tenant Singleton in TinySaas vs OrchardCore

|      Method |      Job |  Runtime |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------ |--------- |--------- |---------:|---------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
| OrchardCore | .NET 6.0 | .NET 6.0 | 64.79 μs | 1.279 μs | 2.029 μs |  1.00 |    0.00 |  8.7891 |      - |   17.9 KB |        1.00 |
|    TinySaas | .NET 6.0 | .NET 6.0 | 54.52 μs | 1.081 μs | 2.547 μs |  0.85 |    0.05 | 10.2539 |      - |  20.85 KB |        1.16 |
|             |          |          |          |          |          |       |         |         |        |           |             |
| OrchardCore | .NET 7.0 | .NET 7.0 | 59.70 μs | 1.185 μs | 3.036 μs |  1.00 |    0.00 |  2.8076 |      - |  17.44 KB |        1.00 |
|    TinySaas | .NET 7.0 | .NET 7.0 | 51.37 μs | 1.060 μs | 3.057 μs |  0.87 |    0.07 |  3.2959 | 0.1221 |  20.65 KB |        1.18 |

## Requirements

ASP.NET Core 3.1+

## Changelog

[Learn about the latest improvements][changelog].

## Credits

[Gunnar Peipman](https://gunnarpeipman.com/) and [Michael McKenna](https://michael-mckenna.com/) for their awesome works on Saas in ASP.NET Core.

## Want to help ?

Want to file a bug, contribute some code, or improve documentation? Excellent! Read up on our
guidelines for [contributing][contributing] and then check out one of our issues in the [hotlist: community-help](https://github.com/codeaisy/tinysaas/labels/hotlist%3A%20community-help).

[contributing]: https://github.com/codeaisy/tinysaas/blob/master/CONTRIBUTING.md
[changelog]: https://github.com/codeaisy/tinysaas/blob/master/CHANGELOG.md
[nuget_link]: https://www.nuget.org/packages/CodEaisy.TinySaas
[nuget_badge]: https://buildstats.info/nuget/codeaisy.tinysaas?includePreReleases=true
[coverage_link]: https://sonarcloud.io/dashboard?id=CodEaisy_TinySaas
[coverage_badge]: https://sonarcloud.io/api/project_badges/measure?project=CodEaisy_TinySaas&metric=coverage
[build_link]: https://github.com/codEaisy/tinysaas/actions/workflows/release.yml
[build_badge]: https://github.com/codEaisy/tinysaas/actions/workflows/release.yml/badge.svg
