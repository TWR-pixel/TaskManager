using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Users.Requests.Identity.Common.Security.SymmetricSecurityKeys;

public interface ISymmetricSecurityKeysGenerator
{
    public SymmetricSecurityKey Create(string securityKey);
}
