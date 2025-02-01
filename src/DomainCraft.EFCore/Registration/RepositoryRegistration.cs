using DomainCraft.EFCore.Abstractions;
using DomainCraft.EFCore.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace DomainCraft.EFCore.DependencyInjection;

public static class RepositoryRegistration
{
    public static IServiceCollection AddDomainCraftRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}