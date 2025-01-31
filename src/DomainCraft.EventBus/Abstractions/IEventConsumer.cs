namespace DomainCraft.EventBus.Abstractions;

public interface IEventConsumer
{
    Task ConsumeAsync<T>(Func<T, Task> handler, string queueName = null) where T : class, IEvent;
}