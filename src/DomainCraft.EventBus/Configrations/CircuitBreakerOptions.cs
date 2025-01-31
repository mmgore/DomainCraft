namespace DomainCraft.EventBus.Configrations;

public class CircuitBreakerOptions
{
    public int ExceptionsAllowedBeforeBreaking { get; set; } = 5;
    public int DurationOfBreakInMinutes { get; set; } = 1;
}