using FluentAssertions;
using IntegrationTests;
using Xunit;

namespace EnterpriseSecureProxy.tests.IntegrationTests
{

    public sealed class StartupTests
     : IClassFixture<ProxyApiFactory>
    {
        private readonly HttpClient _client;

        public StartupTests(
            ProxyApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Application_Should_Start()
        {
            var response =
                await _client.GetAsync("/swagger");

            response.Should().NotBeNull();
        }
    }
}
