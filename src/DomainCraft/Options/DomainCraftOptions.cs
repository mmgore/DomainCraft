using DomainCraft.EventBus.Configrations;
using DomainCraft.RedisCaching.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DomainCraft.Options;

public class DomainCraftOptions
{
    public bool UseEFCoreRepositories { get; set; } = false;
    public bool UseCaching { get; set; } = false;
    public bool UseEventBus { get; set; } = false;
    public bool UseLogging { get; set; } = false;

    public IConfiguration? Configuration { get; set; }
    public Action<CacheOptions>? ConfigureCache { get; set; }
    public Action<RabbitMqOptions>? ConfigureRabbitMq { get; set; }
    public Action<CircuitBreakerOptions>? ConfigureCircuitBreaker { get; set; }
    public Action<RetryOptions>? ConfigureRetry { get; set; }
    
}