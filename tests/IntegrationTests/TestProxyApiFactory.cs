using IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection; // Add this using directive
using System.Net.Http;
public sealed class TestProxyApiFactory
    : ProxyApiFactori
{
    public RecordingHttpMessageHandler Handler
    { get; } = new();

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Manually remove all IHttpClientFactory registrations
            var descriptors = services
                .Where(d => d.ServiceType == typeof(IHttpClientFactory))
                .ToList();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IHttpClientFactory>(
                _ => new FakeHttpClientFactory(
                    Handler));
        });
    }
}