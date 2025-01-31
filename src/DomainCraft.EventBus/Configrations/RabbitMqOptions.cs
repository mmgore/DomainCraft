namespace DomainCraft.EventBus.Configrations;

public class RabbitMqOptions
{
    public string Host { get; set; } = "rabbitmq://localhost";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string QueueName { get; set; } = "default-queue";
}