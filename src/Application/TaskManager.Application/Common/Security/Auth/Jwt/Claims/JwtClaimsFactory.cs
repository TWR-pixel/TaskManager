using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Auth.Jwt.Claims;

public sealed class JwtClaimsFactory : IJwtClaimsFactory
{
    public IEnumerable<Claim> CreateDefault(int userId, int roleId, string username, string roleName)
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
