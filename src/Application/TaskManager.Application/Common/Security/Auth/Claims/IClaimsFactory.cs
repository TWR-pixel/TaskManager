using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Auth.Claims;

public interface IClaimsFactory
{
    public IEnumerable<Claim> CreateDefault(int userId, int roleId, string userName, string roleName);
}
