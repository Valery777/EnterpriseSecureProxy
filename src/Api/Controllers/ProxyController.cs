using EnterpriseSecureProxy.src.Api.Contracts;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Application.Proxy.DTO; // Add this using directive
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/proxy")]

/// <summary>
/// Controller for handling proxy requests.
/// </summary>  
public sealed class ProxyController : ControllerBase
{
    private readonly IProxyExecutionService _proxyExecutionService;
    //private readonly ILogger<ProxyController> _logger;
    public ProxyController(
        IProxyExecutionService proxyExecutionService)//,
        //ILogger<ProxyController> logger)
    {
        _proxyExecutionService = proxyExecutionService;
        //_logger = logger;
    }

    [HttpPost("execute")]
    public async Task Execute(
        [FromBody] EnterpriseSecureProxy.src.Application.Proxy.DTO.ExecuteProxyRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            //_logger.LogInformation("proxyExecutionService started.");
           
            ArgumentNullException.ThrowIfNull(request);
            var result =
                await _proxyExecutionService.ExecuteAsync(
                    request,
                    Request.Headers,
                    cancellationToken);

            Response.StatusCode = result.StatusCode;

            //_logger.LogInformation("proxyExecutionService ended with status code: {StatusCode}", 
             //   result.StatusCode);
           
            if (!string.IsNullOrWhiteSpace(result.ContentType))
            {
                Response.ContentType = result.ContentType;
            }

            if (result.ResponseStream is not null)
            {
                await result.ResponseStream.CopyToAsync(
                    Response.Body,
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error executing proxy request: {Message}", ex.Message);
           
            Console.Error.WriteLine($"Error executing proxy request: {ex.Message}");
            throw;
        }
    }
}