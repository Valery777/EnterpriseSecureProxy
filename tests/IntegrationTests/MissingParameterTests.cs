using FluentAssertions;

using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests
{
    public sealed class MissingParameterTests
    : IClassFixture<ProxyApiFactori>
    {
        private readonly HttpClient _client;

        public MissingParameterTests(
            ProxyApiFactori factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Missing_Required_Parameter_Should_Be_Rejected()
        {
            var request = new
            {
                clientId = "operation.with.required.parameters"
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