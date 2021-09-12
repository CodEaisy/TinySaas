using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CodEaisy.TinySaas.Interfaces;
using CodEaisy.TinySaas.Resolvers;
using CodEaisy.TinySaas.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CodEaisy.TinySaas.Samples.WebApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodEaisy.TinySaas.Tests.Resolvers
{
    public class HostResolutionStrategyTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;
        private const string url = "http://e5353c3f-36bf-43ba-a3ca-6827fecfe558/app";

        public HostResolutionStrategyTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.UpdateDependencyInjection(new List<DependencyUpdate> {
                new DependencyUpdate(typeof(ITenantResolutionStrategy), typeof(HostResolutionStrategy)),
                new DependencyUpdate(typeof(ILoggerFactory), typeof(NullLoggerFactory))
            });
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task ShouldRedirect_WhenHostIs_NotValid()
        {
            // Act
            var response = await _client.GetAsync("http://hello.com/home/app");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task ShouldResolve_WhenHostIs_Valid()
        {
            // Arrange
            const string tenantId = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";
            _client.DefaultRequestHeaders.Add(MultitenancyConstants.TenantIdKey, tenantId);

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
