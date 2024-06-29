using Models.Security;

namespace Services.Abstractions;

public interface IServiceSecurity
{
    public Task<DTOToken> CreateTokenAsync(DTOLogin user);
}