using Admin.Domain.Contracts.Security;
using Admin.Domain.Repository.Abstractions;
using Admin.Services.Abstractions;
using BuildingBase.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Admin.Services;

[AllowAnonymous]
public sealed class ServiceSecurity(IConfiguration configuration, ILogger<ServiceSecurity> logger, IMapper mapper, IStringLocalizer localizer,
                                    IRepositoryManager repositoryManager) : ServiceBase(configuration, logger, mapper, localizer), IServiceSecurity
{
    public async Task<DTOToken> CreateTokenAsync(DTOLogin user)
    {
        DTOToken result;

        //Validation user        
        if (!(user.UserName == "string" && user.Password == "string"))
            throw new ModelStateError("messageLogonFailed");

        //Create token
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    //new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, "gmanfredonia@inwind.it"),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
            Expires = DateTime.UtcNow.AddSeconds(EXPIRES_IN),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.Aes128CbcHmacSha256)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        result = new DTOToken { Token = $"{tokenHandler.WriteToken(token)}" };

        return await Task.FromResult(result).ConfigureAwait(false);
    }

    public const int EXPIRES_IN = 5 * 60;   //Seconds

    private readonly IRepositoryManager repositoryManager = repositoryManager;
}