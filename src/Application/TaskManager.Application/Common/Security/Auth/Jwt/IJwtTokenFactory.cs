using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Common.Security.Auth;

namespace TaskManager.Application.Common.Security.Auth.Jwt;

public interface IJwtTokenFactory : ITokenFactory<JwtSecurityToken>
{
    /// <summary>
    /// Generates token
    /// </summary>
    /// <param name="claims">claims for authentication</param>
    /// <returns>new JWT token</returns>
    public JwtSecurityToken Create(IEnumerable<Claim> claims);
}
