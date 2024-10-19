using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Common.Security.SymmetricSecurityKeys;

public interface ISymmetricSecurityKeysGenerator
{
    public SymmetricSecurityKey Create(string securityKey);
}
