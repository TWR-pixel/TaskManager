using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Users.Requests.Common.Security.SymmetricSecurityKeys;

public interface ISymmetricSecurityKeysGenerator
{
    public SymmetricSecurityKey CreateSecurityKey(string securityKey);
}
