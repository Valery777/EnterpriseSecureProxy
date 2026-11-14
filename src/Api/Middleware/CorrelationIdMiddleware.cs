namespace EnterpriseSecureProxy.src.Api.Middleware
{
    public sealed class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-Id";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                string correlationId;

                if (context.Request.Headers.TryGetValue(
                    HeaderName,
                    out var existingCorrelationId) &&
                    !string.IsNullOrWhiteSpace(existingCorrelationId))
                {
                    correlationId = existingCorrelationId!;
                }
                else
                {
                    correlationId = Guid.NewGuid().ToString("N");
                }

                context.TraceIdentifier = correlationId;

                context.Items[HeaderName] = correlationId;

                context.Response.OnStarting(() =>
                {
                    context.Response.Headers[HeaderName] =
                        correlationId;

                    return Task.CompletedTask;
                });

                await _next(context);
            }
            catch (Exception exception)
            {
                var correlationId =
                    context.TraceIdentifier;
                var logger =
                    context.RequestServices.GetRequiredService<ILogger<CorrelationIdMiddleware>>();
                logger.LogError(
                    exception,
                    "Unhandled exception in CorrelationIdMiddleware. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }
    }
}
