using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Application.Common.Security.Auth.Jwt;

namespace TaskManager.Infrastructure.Security.Jwt;

public sealed class JwtTokenFactory(IOptions<JwtAuthenticationOptions> authOptions) : IJwtTokenFactory
{
    private readonly JwtAuthenticationOptions _authOptions = authOptions.Value;

    public JwtSecurityToken Create(IEnumerable<Claim> claims)
    {
        if (claims is null || !claims.Any())
            throw new ArgumentNullException(nameof(claims));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 

        var token = new JwtSecurityToken(_authOptions.Issuer,
                                         _authOptions.Audience,
                                         claims,
                                         expires: DateTime.UtcNow.AddHours(_authOptions.ExpiresTokenHours),
                                         signingCredentials: signingCredentials);

        return token;
    }


}
