using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Application.Common.Security.Authentication.Abstractions;

namespace TaskManager.Application.Common.Security.SymmetricSecurityKeys;

public sealed class SymmetricSecurityKeysGenerator : ISymmetricSecurityKeysGenerator
{
    public SymmetricSecurityKey CreateSecurityKey(string securityKey)
    {
        if (string.IsNullOrWhiteSpace(securityKey))
            throw new ArgumentException($"\"{nameof(securityKey)}\" не может быть пустым или содержать только пробел.", nameof(securityKey));

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}
