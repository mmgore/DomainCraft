using DomainCraft.RedisCaching.Abstractions;
using DomainCraft.RedisCaching.Implementations;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DomainCraft.RedisCaching.DependencyInjection;

public static class CacheRegistration
{
    public static IServiceCollection AddDomainCraftCache(this IServiceCollection services,
        Action<CacheOptions> configureOptions)
    {
        var options = new CacheOptions();
        configureOptions(options);

        var connectionMultiplexer = ConnectionMultiplexer.Connect(options.ConnectionString);

        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddScoped<ICacheService, CacheService>(sp => new CacheService(sp.GetRequiredService<IConnectionMultiplexer>(), options.Database));
        return services;
    }
}