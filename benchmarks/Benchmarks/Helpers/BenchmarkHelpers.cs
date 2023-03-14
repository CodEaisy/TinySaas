using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace Benchmarks.Helpers
{
    public static class BenchmarkHelpers
    {
        public static readonly string[] Tenants = new string[] {
            "e5353c3f-36bf-43ba-a3ca-6827fecfe558",
            "f6811558-035f-48dd-9321-130f46cb94c6"
        };
        
        public static HttpClientWrapper<TStartup> GetHttpClientWrapper<TStartup>(string displayName, bool baseline = false)
            where TStartup : class => GetHttpClientWrapper<TStartup, HttpClientWrapper<TStartup>>(displayName, baseline);

        public static HttpClientWrapper<TStartup> GetHttpClientWrapper<TStartup, TWrapper>(string displayName, bool baseline = false)
            where TWrapper : HttpClientWrapper<TStartup>, new()
            where TStartup : class
        {
            var factory = new WebApplicationFactory<TStartup>();
            return new TWrapper
            {
                Name = displayName,
                ClientFactory = factory.WithWebHostBuilder(builder => {
                    builder.ConfigureTestServices(services => {
                        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
                    });
                }),
                Baseline = baseline
            };
        }
    }
    
    public interface IHttpClientWrapper
    {
        string Name { get; }
        bool? Baseline { get; }
        Task Call(string path, string tenant);
    }

    public class HttpClientWrapper<TStartup> : IHttpClientWrapper, IDisposable where TStartup : class
    {
        private WebApplicationFactory<TStartup> _clientFactory;
        private HttpClient __httpClient;
        public string Name { get; set; }
        public bool? Baseline { get; set; }
        public WebApplicationFactory<TStartup> ClientFactory
        {
            set => _clientFactory = value;
        }
        public override string ToString() => Name;
        private HttpClient _httpClient {
            get {
                if (__httpClient == null) {
                    __httpClient = _clientFactory.CreateClient(new WebApplicationFactoryClientOptions
                    {
                        AllowAutoRedirect = false
                    });
                }
                return __httpClient;
            }
        }
        
        protected async Task Call(string url)
        {
            using var result = await _httpClient.GetAsync(url);
            result.EnsureSuccessStatusCode();
        }
        
        public virtual Task Call(string method, string tenantId) => Call($"https://localhost:5001/{method}");

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clientFactory.Dispose();
            }
        }
    }
    
    public class TinySaasClient : HttpClientWrapper<TinySaasWebApi.Startup>
    {
        public override Task Call(string method, string tenantId) =>
            Call($"https://localhost:5001/{method}?TinySaasTenantId={tenantId}");
    }
    
    public class OrchardClient : HttpClientWrapper<OrchardCoreWebApi.Startup>
    {
        public override Task Call(string method, string tenantId) =>
            Call($"https://localhost:5001/{tenantId}/{method}");
    }
}
