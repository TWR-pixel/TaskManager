using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Users.Requests.Identity.Common.Security.Auth.Jwt.Options;
using TaskManager.Application.Users.Requests.Identity.Common.Security.SymmetricSecurityKeys;

namespace TaskManager.Application.Users.Requests.Identity.Common.Security.Auth.Jwt.Tokens;

/// <summary>
/// Generate new tokens for JWT authentication
/// </summary>
public sealed class JwtSecurityTokenFactory(IOptions<JwtAuthenticationOptions> authOptions,
    ISymmetricSecurityKeysGenerator symmetricSecurityKeysGenerator) : IJwtSecurityTokenFactory
{
    private readonly IOptions<JwtAuthenticationOptions> _authOptions = authOptions; // options from appsettings.json
    private readonly ISymmetricSecurityKeysGenerator _symmetricSecurityKeysGenerator = symmetricSecurityKeysGenerator; // 

    /// <summary>
    /// Generates token
    /// </summary>
    /// <param name="claims">claims for authentication</param>
    /// <returns>new JWT token</returns>
    public JwtSecurityToken CreateSecurityToken(IEnumerable<Claim> claims)
    {
        if (claims == null || !claims.Any())
            throw new ArgumentNullException(nameof(claims));
        
        var key = _symmetricSecurityKeysGenerator.Create(_authOptions.Value.SecurityKey); // generate new security key 
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // algorithm info

        var token = new JwtSecurityToken(_authOptions.Value.Issuer,
                                         _authOptions.Value.Audience,
                                         claims,
                                         expires: DateTime.UtcNow.AddHours(_authOptions.Value.ExpiresTokenHours),
                                         signingCredentials: signingCredentials);

        return token;
    }


}
