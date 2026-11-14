using FluentAssertions;

using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests
{
    public sealed class DisabledOperationTests
    : IClassFixture<ProxyApiFactory>
    {
        private readonly HttpClient _client;

        public DisabledOperationTests(
            ProxyApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Disabled_Operation_Should_Be_Rejected()
        {
            var request = new
            {
                clientId = "disabled.operation"
            };

            var response =
                await _client.PostAsJsonAsync(
                    "/api/proxy/execute",
                    request);

            response.StatusCode.Should()
                .Be(HttpStatusCode.Forbidden);
        }
    }
}