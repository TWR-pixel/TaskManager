using System.Security.Claims;

namespace TaskManager.Application.Users.Requests.Identity.Common.Security.Auth.Jwt.Claims;

public interface IJwtClaimsFactory
{
    public IEnumerable<Claim> CreateDefault(int userId, int roleId, string userName, string roleName);
}
