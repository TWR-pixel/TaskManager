using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskManager.Application.User.Common.Security.Auth.Tokens.Jwt;

public interface IJwtTokenFactory : ITokenFactory<JwtSecurityToken>
{
    /// <summary>
    /// Generates token
    /// </summary>
    /// <param name="claims">claims for authentication</param>
    /// <returns>new JWT token</returns>
    public JwtSecurityToken Create(IEnumerable<Claim> claims);
}
