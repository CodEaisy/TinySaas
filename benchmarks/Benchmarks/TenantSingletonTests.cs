using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TinySaasWebApi;
using static Benchmarks.BenchmarkHelpers;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net50)]
    public class TenantSingletonTests
    {
        private readonly HttpClientWrapper _tinySaasClient;

        public TenantSingletonTests()
        {
            _tinySaasClient = GetHttpClient<Startup>("TinySaas");
        }

        public IEnumerable<HttpClientWrapper> AppInstances()
        {
            yield return _tinySaasClient;
        }

        [Benchmark]
        [ArgumentsSource(nameof(AppInstances))]
        public async Task HttpGet(HttpClientWrapper Instance) =>
            await Call(Instance.HttpClient, "Tenant", Tenants[0]);
    }
}
