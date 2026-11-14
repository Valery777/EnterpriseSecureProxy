using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Application.Proxy.DTO;
using EnterpriseSecureProxy.src.Application.Proxy.Models;
using EnterpriseSecureProxy.src.Application.Security;
using EnterpriseSecureProxy.src.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
namespace EnterpriseSecureProxy.src.Application.Services
{
    //orchestration layer.
    //    Resolve Operation
    //    ↓
    //    Validate Destination
    //    ↓
    //    Validate Parameters
    //    ↓
    //    Validate Headers
    //    ↓
    //    Validate Encryption
    //    ↓
    //    Audit Start
    //    ↓ 
    //    Forward Request
    //    ↓
    //    Audit Success
    public sealed class ProxyExecutionService
        : IProxyExecutionService
    {
        private readonly IOperationResolver _operationResolver;
        private readonly IDestinationResolver _destinationResolver;
        private readonly IParameterValidator _parameterValidator;
        private readonly IHeaderFilter _headerFilter;
        private readonly IProxyHttpForwarder _httpForwarder;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptionService _encryptionService;
       
        public ProxyExecutionService(
            IOperationResolver operationResolver,
            IDestinationResolver destinationResolver,
            IParameterValidator parameterValidator,
            IHeaderFilter headerFilter,
            IProxyHttpForwarder httpForwarder,
            IAuditService auditService,
            IHttpContextAccessor httpContextAccessor,
            IEncryptionService encryptionService )
           
        {
            _operationResolver = operationResolver;
            _destinationResolver = destinationResolver;
            _parameterValidator = parameterValidator;
            _headerFilter = headerFilter;
            _httpForwarder = httpForwarder;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
            _encryptionService = encryptionService;
     
        }

        public async Task<ProxyExecutionResult> ExecuteAsync(
            ExecuteProxyRequest request,
            IHeaderDictionary headers,
            CancellationToken cancellationToken)
        {
           
           
            // var correlationId = Guid.NewGuid().ToString();
            var correlationId = _httpContextAccessor.HttpContext?.TraceIdentifier  ?? Guid.NewGuid().ToString("N");
            var startedUtc = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var operation =
                    await _operationResolver.ResolveAsync(
                        request.ClientId,
                        cancellationToken);
               
                JsonElement payload;
                if (operation.RequiresEncryption)
                {
                    if (request.EncryptedPayload is null)
                    {
                        
                        throw new InvalidEncryptedPayloadException();
                    }

                    var json =
                        await _encryptionService.DecryptAsync(
                            request.EncryptedPayload,
                            "Default",
                            cancellationToken);

                    using var document =
                        JsonDocument.Parse(json);

                    payload =
                        document.RootElement.Clone();
                }
                else
                {
                    if (request.Payload.ValueKind == JsonValueKind.Undefined)
                    {
                        
                        throw new InvalidParameterException(
                            "Payload is required.");
                    }

                    payload =request.Payload;
                   
                  
                }

                var destination =
                    await _destinationResolver.ResolveAsync(
                        operation.DestinationId,
                        cancellationToken);

                await _parameterValidator.ValidateAsync(
                    operation,
                    request.Payload,
                    cancellationToken);

                var filteredHeaders =
                    _headerFilter.Filter(
                        operation,
                        headers);

                var response =
                    await _httpForwarder.ForwardAsync(
                        operation,
                        destination,
                        request.Payload,
                        filteredHeaders,
                        cancellationToken);

                stopwatch.Stop();
              
               
              
                await _auditService.LogSuccessAsync(
                    new AuditContext
                    {
                        CorrelationId = correlationId,
                        OperationKey = operation.OperationKey,
                        DestinationId = destination.Id,

                        StartedUtc = startedUtc,
                        CompletedUtc = DateTime.UtcNow,

                        DurationMs = stopwatch.ElapsedMilliseconds
                    },
                   (int)response.StatusCode,
                   (int)response.StatusCode,
                   cancellationToken);
               
                              
                return new ProxyExecutionResult
                {
                   
                    StatusCode = (int)response.StatusCode,
                    ContentType =
                        response.Content.Headers.ContentType?.ToString(),

                    ResponseStream =
                        await response.Content.ReadAsStreamAsync(
                            cancellationToken)
                };
            }
            catch (InvalidEncryptedPayloadException)
            {
                
                stopwatch.Stop();

                await _auditService.LogRejectedAsync(
                    new AuditContext
                    {
                        CorrelationId = correlationId,
                        OperationKey = request.ClientId
                    },
                    "Invalid encrypted payload.",
                    cancellationToken);

                throw;
            }
            catch (UpstreamTimeoutException)
            {
               
                stopwatch.Stop();

                await _auditService.LogTimeoutAsync(
                        new AuditContext
                        {
                            CorrelationId = correlationId,
                            OperationKey = request.ClientId
                        },
                        cancellationToken);

                throw;
            }
            catch (Exception ex)
            {
                
                stopwatch.Stop();

                await _auditService.LogFailureAsync(
                    new AuditContext
                    {
                        CorrelationId = correlationId,
                        OperationKey = request.ClientId
                    },
                  
                    ex.Message,
                    cancellationToken);

                throw;
            }
        }
    }
}
