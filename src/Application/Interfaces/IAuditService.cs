using EnterpriseSecureProxy.src.Application.Proxy.Models;

namespace EnterpriseSecureProxy.src.Application.Interfaces;

public interface IAuditService
{
    Task LogSuccessAsync(
        
        AuditContext auditContext,
        int upstreamStatusCode,
        int proxyStatusCode,
        CancellationToken cancellationToken);

    Task LogRejectedAsync(
        AuditContext auditContext,
        string reason,
        CancellationToken cancellationToken);

    Task LogFailureAsync(
        AuditContext auditContext,
        string reason,
        CancellationToken cancellationToken);

    Task LogTimeoutAsync(
        AuditContext auditContext,
        CancellationToken cancellationToken);
}