using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Application.Proxy.Models;
using EnterpriseSecureProxy.src.Domain.Entities;
using EnterpriseSecureProxy.src.Domain.Enums;

namespace EnterpriseSecureProxy.src.Application.Services
{

    //Persist:
    //CorrelationId
    //OperationKey
    //DestinationId
    //StartedUtc
    //CompletedUtc
    //DurationMs
    //Forwarded
    //StatusCode
    //Outcome

    // Never persist:
    //Payload
    //Bearer Token
    //Cookie
    //AES Key
    //Credentials
    public sealed class AuditService : IAuditService
    {
        private readonly IAuditRepository _repository;
       // private readonly ILogger<AuditService> _logger;
        public AuditService(
            IAuditRepository repository)
           
        {
            _repository = repository;
           // _logger = logger;
        }

        //public async Task LogSuccessAsync(
        //    string correlationId,
        //    string operationKey,
        //    Guid destinationId,
        //    int upstreamStatusCode,
        //    int proxyStatusCode,
        //    long durationMs,
        //    CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var log = new RequestLog
        //        {
        //            CorrelationId = correlationId,
        //            OperationKey = operationKey,
        //            DestinationId = destinationId,

        //            StartedUtc = DateTime.UtcNow,
        //            CompletedUtc = DateTime.UtcNow,

        //            DurationMs = durationMs,

        //            Forwarded = true,

        //            UpstreamStatusCode = upstreamStatusCode,
        //            ProxyStatusCode = proxyStatusCode,

        //            Outcome = RequestOutcome.Success
        //        };

        //        await _repository.AddAsync(
        //            log,
        //            cancellationToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to log success");
        //    }
        //}
        public async Task LogSuccessAsync(
               AuditContext context,
               int upstreamStatusCode,
               int proxyStatusCode,
               CancellationToken cancellationToken)
        {
          
            
            var log = new RequestLog
            {
                CorrelationId = context.CorrelationId,
                OperationKey = context.OperationKey,
                DestinationId = context.DestinationId,

                StartedUtc = context.StartedUtc,
                CompletedUtc = context.CompletedUtc,

                DurationMs = context.DurationMs,

                Forwarded = true,

                UpstreamStatusCode = upstreamStatusCode,
                ProxyStatusCode = proxyStatusCode,

                Outcome = RequestOutcome.Success
            };

            await _repository.AddAsync(
                log,
                cancellationToken);
            
           
        }
        public async Task LogRejectedAsync(
            AuditContext context,
            string reason,
            CancellationToken cancellationToken)
        {
            try
            {
             
               
                var log = new RequestLog
                {
                    CorrelationId = context.CorrelationId,
                    OperationKey = context.OperationKey,

                    StartedUtc = DateTime.UtcNow,
                    CompletedUtc = DateTime.UtcNow,

                    Forwarded = false,

                    Outcome = RequestOutcome.Rejected,

                    RejectionReason = reason
                };

                await _repository.AddAsync(
                    log,
                    cancellationToken);

         
                    
            }
            catch (Exception ex)
            {
   
                Console.WriteLine($"Failed to log rejection: {ex.Message}");
            }
        }

        public async Task LogFailureAsync(
            AuditContext context,
            string reason,
            CancellationToken cancellationToken)
        {
            try
            {
                               
                var log = new RequestLog
                {
                    CorrelationId = context.CorrelationId,
                    OperationKey = context.OperationKey,

                    StartedUtc = DateTime.UtcNow,
                    CompletedUtc = DateTime.UtcNow,

                    Forwarded = true,

                    Outcome = RequestOutcome.Failed,

                    RejectionReason = reason
                };

                await _repository.AddAsync(
                    log,
                    cancellationToken);
               
               
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Failed to log failure: {ex.Message}");

            }
        }
        public async Task LogTimeoutAsync(
            AuditContext context,
            CancellationToken cancellationToken)
        {
            try
            {
                

                var log = new RequestLog
                {
                    CorrelationId = context.CorrelationId,
                    OperationKey = context.OperationKey,

                    StartedUtc = DateTime.UtcNow,
                    CompletedUtc = DateTime.UtcNow,

                    Forwarded = true,

                    Outcome = RequestOutcome.Timeout,

                    RejectionReason = "Upstream timeout."
                };

                await _repository.AddAsync(
                    log,
                    cancellationToken);
                
                
            }
            catch (Exception ex)
            {
               
                // Log the exception to a logging framework or console
                Console.WriteLine($"Failed to log timeout: {ex.Message}");
            }
        }
    }
}