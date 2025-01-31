namespace DomainCraft.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<T>(T message) where T : class, IEvent;
}