using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Authentication;

public interface IJwtClaimsFactory
{
    public IEnumerable<Claim> CreateDefault(int userId, int roleId, string userName, string roleName);
}
