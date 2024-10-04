using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Authentication.Abstractions;

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
