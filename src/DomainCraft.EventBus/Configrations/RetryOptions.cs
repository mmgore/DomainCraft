namespace DomainCraft.EventBus.Configrations;

public class RetryOptions
{
    public int RetryCount { get; set; } = 3;
    public int ExponentialBackoffExponent { get; set; } = 2;
}