using Admin.Domain.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.Persistence.Database.Repositories;

namespace Products.Persistence.Database;

public sealed class RepositoryWriting<TContext>(IServiceProvider serviceProvider, TContext dbContext) : UnitOfWork<TContext>(dbContext), IRepositoryWriting where TContext : DbContext
{    
    public IRepositoryProducts Products => repositoryProducts.Value;
    
    private readonly Lazy<IRepositoryProducts> repositoryProducts = new(ActivatorUtilities.CreateInstance<RepositoryProducts>(serviceProvider, dbContext));
}