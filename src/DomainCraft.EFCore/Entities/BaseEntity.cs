namespace DomainCraft.EFCore.Entities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}