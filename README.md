# TinySaas

[![Build](https://github.com/codeaisy/tinysaas/workflows/Build/badge.svg)](https://github.com/CodEaisy/TinySaas/actions?query=workflow%3ABuild)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=CodEaisy_TinySaas&metric=coverage)](https://sonarcloud.io/dashboard?id=CodEaisy_TinySaas)
[Nuget (with prereleases)][nuget_link]

TinySaas is a C# library for building multitenant applications with .NET Core 3.0+

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
dotnet add package CodEaisy.TinySaas.AspNetCore --version 1.0.0
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

## Benchmarks

Here, we show the performance report of an application singleton in a default ASP.NET application and an application singleton in a TinySaas ASP.NET application.

```ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1645 (21H2)
Intel Core i7-10750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.104
  [Host]        : .NET Core 3.1.24 (CoreCLR 4.700.22.16002, CoreFX 4.700.22.17909), X64 RyuJIT
  .NET 5.0      : .NET 5.0.16 (5.0.1622.16705), X64 RyuJIT
  .NET 6.0      : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  .NET Core 3.1 : .NET Core 3.1.24 (CoreCLR 4.700.22.16002, CoreFX 4.700.22.17909), X64 RyuJIT
```

### App Singleton in Default ASP.NET vs TinySaas vs OrchardCore

```ini
|  Method |           Job |       Runtime |    Instance |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|-------- |-------------- |-------------- |------------ |---------:|---------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|
| HttpGet |      .NET 5.0 |      .NET 5.0 |     Default | 44.85 μs | 0.888 μs | 1.795 μs | 44.52 μs |  0.88 |    0.07 | 2.1362 | 0.0610 |     13 KB |
| HttpGet |      .NET 6.0 |      .NET 6.0 |     Default | 36.95 μs | 0.709 μs | 0.728 μs | 36.74 μs |  0.68 |    0.03 | 1.7090 |      - |     11 KB |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 |     Default | 51.25 μs | 1.016 μs | 2.511 μs | 51.18 μs |  1.00 |    0.00 | 2.1973 |      - |     14 KB |
|         |               |               |             |          |          |          |          |       |         |        |        |           |
| HttpGet |      .NET 5.0 |      .NET 5.0 | OrchardCore | 74.14 μs | 1.463 μs | 1.566 μs | 74.18 μs |  0.97 |    0.02 | 3.0518 |      - |     19 KB |
| HttpGet |      .NET 6.0 |      .NET 6.0 | OrchardCore | 64.73 μs | 0.719 μs | 0.672 μs | 64.79 μs |  0.84 |    0.02 | 2.9297 |      - |     18 KB |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 | OrchardCore | 76.75 μs | 1.132 μs | 1.059 μs | 76.67 μs |  1.00 |    0.00 | 3.1738 | 0.2441 |     20 KB |
|         |               |               |             |          |          |          |          |       |         |        |        |           |
| HttpGet |      .NET 5.0 |      .NET 5.0 |    TinySaas | 61.34 μs | 1.210 μs | 2.388 μs | 60.22 μs |  0.91 |    0.03 | 3.6621 | 0.1221 |     23 KB |
| HttpGet |      .NET 6.0 |      .NET 6.0 |    TinySaas | 55.07 μs | 0.670 μs | 0.594 μs | 54.92 μs |  0.80 |    0.01 | 3.4180 | 0.1221 |     21 KB |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 |    TinySaas | 69.18 μs | 0.677 μs | 0.601 μs | 69.23 μs |  1.00 |    0.00 | 3.7842 | 0.1221 |     23 KB |
```

### Tenant Singleton in TinySaas vs OrchardCore

```ini
|  Method |           Job |       Runtime |    Instance |     Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|-------- |-------------- |-------------- |------------ |---------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|
| HttpGet |      .NET 5.0 |      .NET 5.0 | OrchardCore | 76.53 μs | 1.508 μs | 2.114 μs |  1.03 |    0.03 | 3.0518 |      - |     19 KB |
| HttpGet |      .NET 6.0 |      .NET 6.0 | OrchardCore | 65.40 μs | 1.281 μs | 1.258 μs |  0.86 |    0.02 | 2.9297 |      - |     18 KB |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 | OrchardCore | 75.71 μs | 0.729 μs | 0.682 μs |  1.00 |    0.00 | 3.1738 |      - |     20 KB |
|         |               |               |             |          |          |          |       |         |        |        |           |
| HttpGet |      .NET 5.0 |      .NET 5.0 |    TinySaas | 63.72 μs | 1.265 μs | 1.406 μs |  0.90 |    0.02 | 3.6621 | 0.1221 |     23 KB |
| HttpGet |      .NET 6.0 |      .NET 6.0 |    TinySaas | 54.72 μs | 0.613 μs | 0.543 μs |  0.77 |    0.02 | 3.4180 | 0.1221 |     21 KB |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 |    TinySaas | 71.04 μs | 1.380 μs | 1.589 μs |  1.00 |    0.00 | 3.7842 | 0.1221 |     23 KB |
```

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
