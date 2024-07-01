using Admin.Domain.Contracts;
using Admin.Domain.Entities;

namespace Admin.Domain.Repository.Abstractions;

public interface IRepositoryUsers : IRepository<User> 
{
    Task<bool> LoginAsync(DTOLogin login);    
}