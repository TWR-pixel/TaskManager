using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Common.Security.Auth.Options.Jwt;

namespace TaskManager.Application.Common.Security.Auth.Tokens.Jwt;

/// <summary>
/// Generate new tokens for JWT authentication
/// </summary>
public sealed class JwtSecurityTokenFactory(IOptions<JwtAuthenticationOptions> authOptions) : IJwtSecurityTokenFactory
{
    private readonly JwtAuthenticationOptions _authOptions = authOptions.Value; // options from appsettings.json

    /// <summary>
    /// Generates token
    /// </summary>
    /// <param name="claims">claims for authentication</param>
    /// <returns>new JWT token</returns>
    public JwtSecurityToken Create(IEnumerable<Claim> claims)
    {
        if (claims == null || !claims.Any())
            throw new ArgumentNullException(nameof(claims));

        var key = _authOptions.SecurityKeysGenerator.Create(_authOptions.SecretKey); // generate new security key 
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // algorithm info

        var token = new JwtSecurityToken(_authOptions.Issuer,
                                         _authOptions.Audience,
                                         claims,
                                         expires: DateTime.UtcNow.AddHours(_authOptions.ExpiresTokenHours),
                                         signingCredentials: signingCredentials);

        return token;
    }


}
