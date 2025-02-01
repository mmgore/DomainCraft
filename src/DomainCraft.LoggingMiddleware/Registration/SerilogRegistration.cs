using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace DomainCraft.LoggingMiddleware.Registration;

public static class SerilogRegistration
{
    public static void AddDomainCraftLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var elasticUri = configuration["ElasticConfiguration:Uri"];
            
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithCorrelationId()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "domaincraft-logs-{0:yyyy.MM.dd}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            })
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }

}