using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Users.Requests.Common.Security.Authentication;

namespace TaskManager.Application.Users.Requests.Common.Security.Authentication.JwtAuth.JwtTokens;

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
