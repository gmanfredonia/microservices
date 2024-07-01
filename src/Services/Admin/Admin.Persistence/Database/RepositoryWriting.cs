using Admin.Domain.Repository.Abstractions;
using Admin.Persistence.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Persistence.Database;

public sealed class RepositoryWriting<TContext>(IServiceProvider serviceProvider, TContext dbContext) : UnitOfWork<TContext>(dbContext), IRepositoryWriting where TContext : DbContext
{    
    public IRepositoryUsers Users => repositoryUsers.Value;
    
    private readonly Lazy<IRepositoryUsers> repositoryUsers = new(ActivatorUtilities.CreateInstance<RepositoryUsers>(serviceProvider, dbContext));
}