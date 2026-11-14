using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace IntegrationTests
{
    public sealed class PathTraversalTests
    : IClassFixture<ProxyApiFactori>
    {
        private readonly HttpClient _client;

        public PathTraversalTests(
            ProxyApiFactori factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Path_Traversal_Should_Be_Rejected()
        {
            var request = new
            {
                clientId = "test.operation",
                path = "../../../etc/passwd"
            };

            var response =
                await _client.PostAsJsonAsync(
                    "/api/proxy/execute",
                    request);

            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }
    }
}
