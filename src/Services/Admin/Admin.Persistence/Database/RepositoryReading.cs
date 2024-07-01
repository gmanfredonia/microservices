using Admin.Domain.Repository.Abstractions;
using Admin.Persistence.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Persistence.Database;

public sealed class RepositoryReading<TDBContext>(IServiceProvider serviceProvider) : IRepositoryReading where TDBContext : DbContext
{
    public IRepositoryUsers UsersGetInstance()
    {
        TDBContext dbContext = ActivatorUtilities.CreateInstance<TDBContext>(serviceProvider);
        return ActivatorUtilities.CreateInstance<RepositoryUsers>(serviceProvider, dbContext);
    }

    private readonly IServiceProvider serviceProvider = serviceProvider;
}