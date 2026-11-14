using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace IntegrationTests
{
    public sealed class InvalidEncryptedPayloadTests
    : IClassFixture<ProxyApiFactory>
    {
        private readonly HttpClient _client;

        public InvalidEncryptedPayloadTests(
            ProxyApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Invalid_Encrypted_Payload_Should_Be_Rejected()
        {
            var request = new
            {
                clientId = "encrypted.operation",
                encryptedPayload = "invalid-cipher-text"
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