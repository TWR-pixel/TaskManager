using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.User.Common.Security.SymmetricSecurityKeys;

public interface IKeysGenerator
{
    public SymmetricSecurityKey Create(string securityKey);
}
