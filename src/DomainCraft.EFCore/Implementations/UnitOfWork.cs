using System.Collections;
using DomainCraft.EFCore.Abstractions;
using DomainCraft.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainCraft.EFCore.Implementations;

public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly Hashtable _repositories;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<TEntity, TKey>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity, TKey>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null) return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to commit.");

            await _context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to roll back.");

            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }