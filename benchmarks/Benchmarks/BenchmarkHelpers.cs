using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Benchmarks
{
    public static class BenchmarkHelpers
    {
        public static readonly string[] Tenants = new string[] {
            "e5353c3f-36bf-43ba-a3ca-6827fecfe558",
            "f6811558-035f-48dd-9321-130f46cb94c6"
        };

        public static HttpClientWrapper GetHttpClient<TStartup>(string displayName) where TStartup : class
        {
            var factory = new WebApplicationFactory<TStartup>();
            return new HttpClientWrapper
            {
                Name = displayName,
                HttpClient = factory.WithWebHostBuilder(builder => {
                    builder.ConfigureTestServices(services => {
                        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
                    });
                }).CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                })
            };
        }

        public static async Task Call(HttpClient httpClient, string method, string tenantId)
        {
            var result = await httpClient.GetAsync($"https://localhost:5001/{method}?TinySaasTenantId={tenantId}");
            result.EnsureSuccessStatusCode();
        }

        public struct HttpClientWrapper
        {
            public string Name { get; set; }
            public HttpClient HttpClient { get; set; }
            public override string ToString() => Name;
        }
    }
}
