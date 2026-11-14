using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

using Xunit;
namespace IntegrationTests
{

    public sealed class UnknownOperationTests
        : IClassFixture<ProxyApiFactory>
    {
        private readonly HttpClient _client;

        public UnknownOperationTests(
            ProxyApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Unknown_Operation_Should_Be_Rejected()
        {
            // Arrange
            var request = new
            {
                clientId = "unknown.operation"
            };

            // Act
            var response =
                await _client.PostAsJsonAsync(
                    "/api/proxy/execute",
                    request);

            var content =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"Status: {response.StatusCode}");

            Console.WriteLine(content);

            // Assert
            response.StatusCode.Should()
                .BeOneOf(
                    HttpStatusCode.NotFound,
                    HttpStatusCode.BadRequest);
        }
    }
}