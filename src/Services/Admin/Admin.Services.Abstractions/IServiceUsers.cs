using Admin.Domain.Contracts;

namespace Admin.Services.Abstractions;

public interface IServiceUsers
{
    public Task<DTOToken> CreateTokenAsync(DTOLogin user);
}