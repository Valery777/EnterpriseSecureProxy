using FluentAssertions;

using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests
{
    public sealed class ValidConfiguredOperationTests
    : IClassFixture<ProxyApiFactori>
    {
        private readonly HttpClient _client;

        public ValidConfiguredOperationTests(
            ProxyApiFactori factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(Skip ="CustomerApi intentionally disabled for destination tests")]
    
        public async Task Valid_Operation_Should_Be_Processed()
        {
            var request = new
            {
                clientId = "customer.get",
                payload = new
                {
                    customerId = 123,
                    country = "IL"
                }
            };

            var response =
                await _client.PostAsJsonAsync(
                    "/api/proxy/execute",
                    request);

            var content =await response.Content.ReadAsStringAsync();
                    
            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine(content);
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();
        }
    }
}
