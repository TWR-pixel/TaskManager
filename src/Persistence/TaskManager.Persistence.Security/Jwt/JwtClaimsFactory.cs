using System.Security.Claims;
using TaskManager.Application.Common.Security.Auth;

namespace TaskManager.Persistence.Security.Jwt;

public sealed class JwtClaimsFactory : IClaimsFactory
{
    public IEnumerable<Claim> Create(int userId, int roleId, string username, string roleName)
    {
        var claims = new List<Claim>()
        {
            new(nameof(userId), userId.ToString()),
            new(nameof(roleId), roleId.ToString()),
            new(nameof(username), username),
            new(nameof(roleName), roleName)
        };

        return claims;
    }
}
