using DomainCraft.EventBus.Abstractions;
using DomainCraft.EventBus.Configrations;
using MassTransit;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace DomainCraft.EventBus.Implementations;

public class MassTransitEventBus : IEventBus
{
    private readonly IBus _bus;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private readonly AsyncRetryPolicy _retryPolicy;

    public MassTransitEventBus(
        IBus bus,
        RetryOptions retryOptions,
        CircuitBreakerOptions circuitBreakerOptions)
    {
        _bus = bus;

        // Configure retry policy
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryOptions.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryOptions.ExponentialBackoffExponent, retryAttempt))
            );

        // Configure circuit breaker policy
        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                circuitBreakerOptions.ExceptionsAllowedBeforeBreaking,
                TimeSpan.FromMinutes(circuitBreakerOptions.DurationOfBreakInMinutes)
            );
    }

    public async Task PublishAsync<T>(T message) where T : class, IEvent
    {
        // Combine retry and circuit breaker policies
        var resiliencePolicy = Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy);

        await resiliencePolicy.ExecuteAsync(async () => { await _bus.Publish(message); });
    }
}