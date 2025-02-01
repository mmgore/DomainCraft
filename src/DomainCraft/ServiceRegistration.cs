using DomainCraft.EFCore.DependencyInjection;
using DomainCraft.EventBus.Registrations;
using DomainCraft.LoggingMiddleware.Registration;
using DomainCraft.Options;
using DomainCraft.RedisCaching.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainCraft;

public static class ServiceRegistration
{
    public static IServiceCollection AddDomainCraft<TContext>(this IServiceCollection services, IConfiguration configuration, Action<DomainCraftOptions>? options = null)
    where TContext : DbContext
    {
        var opts = new DomainCraftOptions { Configuration = configuration };
        options?.Invoke(opts);

        if (opts.UseEFCoreRepositories)
            services.AddDomainCraftRepositories<TContext>();

        if (opts is { UseCaching: true, ConfigureCache: not null })
            services.AddDomainCraftCaching(opts.ConfigureCache);

        if (opts is { UseEventBus: true, ConfigureRabbitMq: not null, ConfigureRetry: not null, ConfigureCircuitBreaker: not null })
            services.AddDomainCraftEventBus(opts.ConfigureRabbitMq,opts.ConfigureRetry,opts.ConfigureCircuitBreaker);

        if (opts.UseLogging)
            services.AddDomainCraftLogging(opts.Configuration);

        return services;
    }
}