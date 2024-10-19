using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Common.Security.Auth;

namespace TaskManager.Application.Common.Security.Auth.Jwt.Tokens;

public interface IJwtSecurityTokenFactory : ISecurityTokenFactory<JwtSecurityToken>
{
    /// <summary>
    /// Generates token
    /// </summary>
    /// <param name="claims">claims for authentication</param>
    /// <returns>new JWT token</returns>
    public JwtSecurityToken CreateSecurityToken(
            IEnumerable<Claim> claims
        );
}
