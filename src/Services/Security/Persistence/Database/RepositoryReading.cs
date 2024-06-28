using Domain.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.Repositories;

namespace Persistence.Database;

public sealed class RepositoryReading<TDBContext>(IServiceProvider serviceProvider) : IRepositoryReading where TDBContext : DbContext
{
    public IRepositoryProducts ProductsGetInstance()
    {
        TDBContext dbContext = ActivatorUtilities.CreateInstance<TDBContext>(serviceProvider);
        return ActivatorUtilities.CreateInstance<RepositoryProducts>(serviceProvider, dbContext);
    }

    private readonly IServiceProvider serviceProvider = serviceProvider;
}