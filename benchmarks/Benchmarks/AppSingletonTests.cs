using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using static Benchmarks.BenchmarkHelpers;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net50)]
    public class AppSingletonTests
    {
        private readonly HttpClientWrapper _tinySaasClient;
        private readonly HttpClientWrapper _defaultClient;

        public AppSingletonTests()
        {
            _tinySaasClient = GetHttpClient<TinySaasWebApi.Startup>("TinySaas");
            _defaultClient = GetHttpClient<DefaultWebApi.Startup>("Default");
        }

        public IEnumerable<HttpClientWrapper> AppInstances()
        {
            yield return _tinySaasClient;
            yield return _defaultClient;
        }

        [Benchmark]
        [ArgumentsSource(nameof(AppInstances))]
        public async Task HttpGet(HttpClientWrapper Instance) =>
            await Call(Instance.HttpClient, "App", Tenants[0]);
    }
}
