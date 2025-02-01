namespace DomainCraft.EFCore.Entities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
}