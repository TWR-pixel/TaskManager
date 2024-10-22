using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Auth.Claims.Jwt;

public sealed class JwtClaimsFactory : IClaimsFactory
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
