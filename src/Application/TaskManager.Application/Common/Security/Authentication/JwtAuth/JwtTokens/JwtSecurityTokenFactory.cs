using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Common.Security.Authentication.Abstractions;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.Options;

namespace TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;

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

        var key = _symmetricSecurityKeysGenerator.CreateSecurityKey(_authOptions.Value.SecurityKey); // generate new security key 
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // data for algorithm info

        var token = new JwtSecurityToken(
            _authOptions.Value.Issuer,
            _authOptions.Value.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(_authOptions.Value.ExpiresTokenHours),
            signingCredentials: signingCredentials
            );

        return token;
    }


}
