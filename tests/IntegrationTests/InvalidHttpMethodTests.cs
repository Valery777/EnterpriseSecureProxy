using FluentAssertions;

using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests
{
    public sealed class InvalidHttpMethodTests
    : IClassFixture<ProxyApiFactory>
    {
        private readonly HttpClient _client;

        public InvalidHttpMethodTests(
            ProxyApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Invalid_Method_Should_Be_Rejected()
        {
            var request = new
            {
                clientId = "invalid.method.operation"
            };

            var response =
                await _client.PostAsJsonAsync(
                    "/api/proxy/execute",
                    request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }
    }
}
