using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomainCraft.LoggingMiddleware.Middlewares;

public class LoggingMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Read the request body
        var requestBody = await ReadRequestBody(context);

        // Log the request details
        _logger.LogInformation("Incoming Request: {Method} {Path} | Body: {Body}",
            context.Request.Method, context.Request.Path, requestBody);

        // Store original response stream
        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        // Call the next middleware
        await _next(context);

        // Read response body
        var responseBody = await ReadResponseBody(context);
        stopwatch.Stop();

        // Log the response details
        _logger.LogInformation("Outgoing Response: {StatusCode} | Time: {ElapsedMs}ms | Body: {Body}",
            context.Response.StatusCode, stopwatch.ElapsedMilliseconds, responseBody);

        // Copy response back to original stream
        await responseBodyStream.CopyToAsync(originalResponseBodyStream);
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0; // Reset stream position
        return body;
    }

    private async Task<string> ReadResponseBody(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}