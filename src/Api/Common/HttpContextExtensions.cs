using EnterpriseSecureProxy.src.Api.Middleware;

namespace EnterpriseSecureProxy.src.Api.Common
{
    public static class HttpContextExtensions
    {
        public static string GetCorrelationId(
            this HttpContext context)
        {
            return context.Items[
                CorrelationIdMiddleware.HeaderName]?
                .ToString()
                ?? context.TraceIdentifier;
        }
    }
}
