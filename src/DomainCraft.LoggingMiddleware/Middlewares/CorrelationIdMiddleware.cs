using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace DomainCraft.LoggingMiddleware.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task Invoke(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);

        // Add correlation ID to Serilog log context
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Add Correlation ID to Response Headers
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            await next(context);
        }
    }

    private string GetOrCreateCorrelationId(HttpContext context)
    {
        return context.Request.Headers.TryGetValue(CorrelationIdHeader, out var existingId) ? existingId.ToString() : Guid.NewGuid().ToString();
    }
}