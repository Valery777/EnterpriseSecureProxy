using FluentAssertions;

using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests
{
    public sealed class ConcurrencyConflictTests
    : IClassFixture<ProxyApiFactory>
    {
        private readonly HttpClient _client;

        public ConcurrencyConflictTests(
            ProxyApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Concurrency_Conflict_Should_Return_409()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}