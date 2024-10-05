using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Authentication;

public sealed class JwtClaimsFactory : IJwtClaimsFactory
{
    public IEnumerable<Claim> CreateDefault(int userId, int roleId, string userName, string roleName)
    {
        var claims = new List<Claim>()
        {
            new("userId", userId.ToString()),
            new("roleId", roleId.ToString())
        };

        return claims;
    }
}
