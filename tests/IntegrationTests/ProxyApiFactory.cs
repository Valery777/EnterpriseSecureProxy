using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;

namespace IntegrationTests;

public sealed class ProxyApiFactory
    : WebApplicationFactory<Program>
{
    //public RecordingHttpMessageHandler Handler
    //{ get; } = new();

    //protected override void ConfigureWebHost(
    //    IWebHostBuilder builder)
    //{
    //    builder.ConfigureServices(services =>
    //    {
    //        services.RemoveAll<IHttpClientFactory>();

    //        services.AddSingleton<IHttpClientFactory>(
    //            _ => new FakeHttpClientFactory(Handler));
    //    });
    //}
}