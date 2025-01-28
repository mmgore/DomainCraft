using System.Linq.Expressions;
using DomainCraft.EFCore.Abstractions;
using DomainCraft.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainCraft.EFCore.Implementations;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> Queryable() => _dbSet;

    public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.Where(predicate);

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) 
        => await _dbSet.FindAsync(new object[] { id }, cancellationToken);

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default) 
        => await _dbSet.ToListAsync(cancellationToken);

    public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) 
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) 
        => await _dbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) 
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default) 
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}