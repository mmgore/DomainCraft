using DomainCraft.EventBus.Abstractions;
using DomainCraft.EventBus.Configrations;
using MassTransit;

namespace DomainCraft.EventBus.Implementations;

public class BaseEventConsumer(IBus bus, RabbitMqOptions rabbitMqOptions) : IEventConsumer
{
    public async Task ConsumeAsync<T>(Func<T, Task> handler, string queueName = null) where T : class, IEvent
    {
        var queue = queueName ?? rabbitMqOptions.QueueName;

        var connectHandle = bus.ConnectReceiveEndpoint(queue,
            cfg => { cfg.Handler<T>(async context => { await handler(context.Message); }); });

        await connectHandle.Ready;
    }
}