using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using CodEaisy.TinySaas.Samples.WebApi;
using CodEaisy.TinySaas.Samples.WebApi.Services;
using CodEaisy.TinySaas.Tests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace CodEaisy.TinySaas.Tests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        private static string GetUrl(string method, string tenantId) => $"http://localhost:5000/{method}?{MultitenancyConstants.TenantIdKey}={tenantId}";

        public HomeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.UpdateDependencyInjection(new List<DependencyUpdate> {
                new DependencyUpdate(typeof(ILoggerFactory), typeof(NullLoggerFactory))
            });
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task App_ShouldReturn_SameInstance()
        {
            // Arrange
            const string tenant1Id = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";
            const string tenant3Id = "f6811558-035f-48dd-9321-130f46cb94c6";
            const string method = "app";

            // Act
            var response1 = await _client.GetAsync(GetUrl(method, tenant1Id));
            var response2 = await _client.GetAsync(GetUrl(method, tenant3Id));

            // Assert
            var result1 = await response1.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            Assert.True(response1.IsSuccessStatusCode);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public async Task Tenant_ShouldReturn_DifferentInstance()
        {
            // Arrange
            const string tenant1Id = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";
            const string tenant3Id = "f6811558-035f-48dd-9321-130f46cb94c6";
            const string method = "tenant";

            // Act
            var response1 = await _client.GetAsync(GetUrl(method, tenant1Id));
            var response2 = await _client.GetAsync(GetUrl(method, tenant3Id));

            // Assert
            var result1 = await response1.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            Assert.True(response1.IsSuccessStatusCode);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public async Task ScopedWithOptions_ShouldReturn_MixedValues()
        {
            // Arrange
            const string tenant1Id = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";
            const string tenant3Id = "f6811558-035f-48dd-9321-130f46cb94c6";
            const string method = "scoped";

            // Act
            var response1 = await _client.GetAsync(GetUrl(method, tenant1Id));
            var response2 = await _client.GetAsync(GetUrl(method, tenant3Id));

            // Assert
            var result1 = await response1.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            var parsed1 = JsonConvert.DeserializeObject<OptionValues>(result1);
            var parsed2 = JsonConvert.DeserializeObject<OptionValues>(result2);

            Assert.True(response1.IsSuccessStatusCode);
            Assert.Equal(parsed1.AppOption, parsed2.AppOption);
            Assert.NotEqual(parsed1.TenantOption, parsed2.TenantOption);
        }

        [Fact]
        public async Task ScopedWithOptions_ShouldReturn_SameValueWhenSameTenant()
        {
            // Arrange
            const string tenantId = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";
            const string method = "scoped";

            // Act
            var response1 = await _client.GetAsync(GetUrl(method, tenantId));
            var response2 = await _client.GetAsync(GetUrl(method, tenantId));

            // Assert
            var result1 = await response1.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            var parsed1 = JsonConvert.DeserializeObject<OptionValues>(result1);
            var parsed2 = JsonConvert.DeserializeObject<OptionValues>(result2);

            Assert.True(response1.IsSuccessStatusCode);
            Assert.Equal(parsed1.TenantOption, parsed2.TenantOption);
        }

        [Fact]
        public async Task App_ShouldRedirect_WhenMissingTenant()
        {
            // Act
            var response = await _client.GetAsync(GetUrl("scoped", string.Empty));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task App_ShouldUseUpdatedKey()
        {
            // Arrange
            MultitenancyConstants.TenantIdKey = "HelloId";
            const string tenantId = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";

            // Act
            var response = await _client.GetAsync(GetUrl("app", tenantId));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Accessor_ShouldReturn_TenantName()
        {
            // Arrange
            const string tenantId = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";

            // Act
            var response = await _client.GetAsync(GetUrl("accessor", tenantId));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("Tenant 1", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Authenticate_ShouldResolveWhenUserIsAuthenticated()
        {
            // Arrange
            const string method = "auth";
            const string tenantId = "e5353c3f-36bf-43ba-a3ca-6827fecfe558";
            _client.DefaultRequestHeaders.Add("Authorization", "notreallyasecret");

            // Act
            var response = await _client.GetAsync(GetUrl(method, tenantId));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Authenticate_ShouldReturn403_WhenUserTrespasses()
        {
            // Arrange
            const string method = "auth";
            const string tenantId = "f6811558-035f-48dd-9321-130f46cb94c6";
            _client.DefaultRequestHeaders.Add("Authorization", "notreallyasecret");

            // Act
            var response = await _client.GetAsync(GetUrl(method, tenantId));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Authenticate_ShouldReturn403_WhenEmptyAuthHeader()
        {
            // Arrange
            const string method = "auth";
            const string tenantId = "f6811558-035f-48dd-9321-130f46cb94c6";

            // Act
            var response = await _client.GetAsync(GetUrl(method, tenantId));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
