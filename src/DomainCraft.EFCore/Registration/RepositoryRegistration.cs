using DomainCraft.EFCore.Abstractions;
using DomainCraft.EFCore.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DomainCraft.EFCore.DependencyInjection;

public static class RepositoryRegistration
{
    public static IServiceCollection AddDomainCraftRepositories<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        return services;
    }
}