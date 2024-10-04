using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Common.Security.Authentication.Abstractions;

public interface ISymmetricSecurityKeysGenerator
{
    public SymmetricSecurityKey CreateSecurityKey(string securityKey);
}
