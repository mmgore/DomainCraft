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
    public static IServiceCollection AddDomainCraft(this IServiceCollection services, IConfiguration configuration, Action<DomainCraftOptions>? options = null)
    {
        var opts = new DomainCraftOptions { Configuration = configuration };
        options?.Invoke(opts);

        if (opts.UseEFCore)
            services.AddDomainCraftRepositories<DbContext>();

        if (opts.UseCaching)
            services.AddDomainCraftCaching(opts.ConfigureCache);

        if (opts.UseEventBus)
            services.AddDomainCraftEventBus(opts.ConfigureRabbitMq,opts.ConfigureRetry,opts.ConfigureCircuitBreaker);

        if (opts.UseLogging)
            services.AddDomainCraftLogging(opts.Configuration);

        return services;
    }

}