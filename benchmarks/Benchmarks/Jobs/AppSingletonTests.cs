using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Benchmarks.Helpers;
using static Benchmarks.Helpers.BenchmarkHelpers;

namespace Benchmarks.Jobs
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    [RPlotExporter]
    public class AppSingletonTests
    {
        private readonly IHttpClientWrapper _tinySaasClient;
        private readonly IHttpClientWrapper _defaultClient;
        private readonly IHttpClientWrapper _orchardCoreClient;
        private static readonly Random _random = new Random();
        
        
        public AppSingletonTests()
        {
            _defaultClient = GetHttpClientWrapper<DefaultWebApi.Startup>("Default");
            _tinySaasClient = GetHttpClientWrapper<TinySaasWebApi.Startup, TinySaasClient>("TinySaas");
            _orchardCoreClient = GetHttpClientWrapper<OrchardCoreWebApi.Startup, OrchardClient>("OrchardCore");
        }

        public IEnumerable<IHttpClientWrapper> AppInstances()
        {
            yield return _defaultClient;
            yield return _tinySaasClient;
            yield return _orchardCoreClient;
        }

        [Benchmark]
        [ArgumentsSource(nameof(AppInstances))]
        public async Task HttpGet(IHttpClientWrapper Instance) =>
            await Instance.Call("App", Tenants[_random.Next(Tenants.Length)]);
    }
}
