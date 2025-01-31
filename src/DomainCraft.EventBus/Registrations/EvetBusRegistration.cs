using DomainCraft.EventBus.Abstractions;
using DomainCraft.EventBus.Configrations;
using DomainCraft.EventBus.Implementations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DomainCraft.EventBus.Registrations;

public static class EvetBusRegistration
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection services,
        Action<RabbitMqOptions> configureRabbitMq,
        Action<RetryOptions> configureRetry,
        Action<CircuitBreakerOptions> configureCircuitBreaker)
    {
        // Configure options
        var rabbitMqOptions = new RabbitMqOptions();
        configureRabbitMq(rabbitMqOptions);

        var retryOptions = new RetryOptions();
        configureRetry(retryOptions);

        var circuitBreakerOptions = new CircuitBreakerOptions();
        configureCircuitBreaker(circuitBreakerOptions);

        // Register MassTransit with RabbitMQ
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqOptions.Host), h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // Register Event Publisher and Consumer
        services.AddSingleton(rabbitMqOptions);
        services.AddSingleton(retryOptions);
        services.AddSingleton(circuitBreakerOptions);
        services.AddTransient<IEventBus, MassTransitEventBus>();
        services.AddTransient<IEventConsumer, BaseEventConsumer>();

        return services;
    }
}