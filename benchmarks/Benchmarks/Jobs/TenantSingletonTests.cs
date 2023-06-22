using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Benchmarks.Helpers;
using static Benchmarks.Helpers.BenchmarkHelpers;

namespace Benchmarks.Jobs
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob)]
    [MemoryDiagnoser]
    [RPlotExporter]
    public class TenantSingletonTests
    {
        private readonly IHttpClientWrapper _tinySaasClient;
        private readonly IHttpClientWrapper _orchardCoreClient;
        private static readonly Random _random = new Random();

        public TenantSingletonTests()
        {
            _tinySaasClient = GetHttpClientWrapper<TinySaasWebApi.Startup, TinySaasClient>("TinySaas");
            _orchardCoreClient = GetHttpClientWrapper<OrchardCoreWebApi.Startup, OrchardClient>("OrchardCore", true);
        }

        // TODO: use this when parameterized benchmark is implemented in BenchmarkDotNet (https://github.com/dotnet/BenchmarkDotNet/issues/881)
        
        // public IEnumerable<IHttpClientWrapper> AppInstances()
        // {
        //     return new[] { _tinySaasClient, _orchardCoreClient };
        // }

        // [Benchmark]
        // [ArgumentsSource(nameof(AppInstances))]
        // public async Task HttpGet(IHttpClientWrapper Instance) =>
        //     await Instance.Call("Tenant", Tenants[_random.Next(Tenants.Length)]);

        [Benchmark(Baseline = true)]
        public async Task OrchardCore() => await _orchardCoreClient.Call("Tenant", Tenants[_random.Next(Tenants.Length)]);

        [Benchmark]
        public async Task TinySaas() => await _tinySaasClient.Call("Tenant", Tenants[_random.Next(Tenants.Length)]);
    }
}
