using Admin.Domain.Contracts;
using Admin.Domain.Entities;
using Admin.Domain.Repository.Abstractions;
using Microsoft.Extensions.Logging;

namespace Admin.Persistence.Database.Repositories;

public sealed class RepositoryUsers(DbContextRateIt dbContext, ILogger<RepositoryUsers> logger) : Repository<DbContextRateIt, User>(dbContext), IRepositoryUsers
{    
    
    public Task<bool> LoginAsync(DTOLogin login)
    {
        throw new NotImplementedException();
    }

    private readonly ILogger<RepositoryUsers> logger = logger;

}