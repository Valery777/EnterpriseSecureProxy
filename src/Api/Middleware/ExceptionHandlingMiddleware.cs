using EnterpriseSecureProxy.src.Api.Contracts;
using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Infrastructure.Exceptions;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EnterpriseSecureProxy.src.Api.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions.
    /// This middleware ensures that exceptions are caught and handled gracefully, 
    /// providing a consistent error response to the client.
    /// Example error response:
    /// {
    /// "code":"OperationDisabled",
    /// "message":"The requested operation is unavailable."
    ///}
    /// Never expose
    ///Stack traces
    ///Connection strings
    ///Internal URLs
    ///Database errors
    ///Encryption details
    /// </summary>   
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware( RequestDelegate next )
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var correlationId =
                 context.TraceIdentifier;

            var response = exception switch
            {
                UnknownOperationException =>
                    CreateResponse(
                        StatusCodes.Status404NotFound,
                        "UnknownOperation",
                        exception.Message,
                        correlationId),

                OperationDisabledException =>
                    CreateResponse(
                        StatusCodes.Status403Forbidden,
                        "OperationDisabled",
                        exception.Message,
                        correlationId),

                DestinationDisabledException =>
                    CreateResponse(
                        StatusCodes.Status503ServiceUnavailable,
                        "DestinationDisabled",
                        exception.Message,
                        correlationId),

                DestinationNotFoundException =>
                    CreateResponse(
                        StatusCodes.Status404NotFound,
                        "DestinationNotFound",
                        exception.Message,
                        correlationId),

                InvalidParameterException =>
                    CreateResponse(
                        StatusCodes.Status400BadRequest,
                        "InvalidParameter",
                        exception.Message,
                        correlationId),

                MissingHeaderException =>
                    CreateResponse(
                        StatusCodes.Status400BadRequest,
                        "MissingHeader",
                        exception.Message,
                        correlationId),
                ConfigurationConcurrencyException =>
                    CreateResponse(
                    StatusCodes.Status409Conflict,
                    "ConfigurationConflict",
                    "Configuration was modified by another process.",
                    correlationId),
                PathTraversalException =>
                    CreateResponse(
                        StatusCodes.Status400BadRequest,
                        "PathTraversalDetected",
                        exception.Message,
                        correlationId),
                InvalidEncryptedPayloadException =>
                        CreateResponse(
                            StatusCodes.Status400BadRequest,
                            "InvalidEncryptedPayload",
                            "Encrypted payload validation failed.",
                            correlationId),
                UpstreamTimeoutException =>
                    CreateResponse(
                        StatusCodes.Status504GatewayTimeout,
                        "UpstreamTimeout",
                        exception.Message,
                        correlationId),

                HttpRequestException =>
                    CreateResponse(
                        StatusCodes.Status502BadGateway,
                        "UpstreamConnectionFailure",
                        "Failed to connect to upstream service.",
                        correlationId),

                _ =>
                    CreateResponse(
                        StatusCodes.Status500InternalServerError,
                        "InternalServerError",
                        "An unexpected error occurred.",
                        correlationId),

            };

            context.Response.StatusCode =
                response.StatusCode;

            context.Response.ContentType =
                "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        private static ErrorResponse CreateResponse(
            int statusCode,
            string code,
            string message,
            string correlationId)
        {
            return new ErrorResponse
            {
                StatusCode = statusCode,
                Code = code,
                Message = message,
                CorrelationId = correlationId
            };
        }
    }
}