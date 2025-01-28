using System.Linq.Expressions;
using DomainCraft.EFCore.Entities;

namespace DomainCraft.EFCore.Abstractions;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    IQueryable<TEntity> Queryable();
    IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}