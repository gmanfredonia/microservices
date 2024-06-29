using Admin.Domain.Contracts.Security;

namespace Admin.Services.Abstractions;

public interface IServiceSecurity
{
    public Task<DTOToken> CreateTokenAsync(DTOLogin user);
}