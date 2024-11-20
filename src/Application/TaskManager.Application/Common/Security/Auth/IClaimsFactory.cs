using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Auth;

public interface IClaimsFactory
{
    public IEnumerable<Claim> Create(int userId, int roleId, string userName, string roleName);
}
