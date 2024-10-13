using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Common.Security.Authentication;

/// <summary>
/// Generates new tokens for JWT authentication
/// </summary>
public interface ISecurityTokenFactory<TSecurityToken> where TSecurityToken : SecurityToken
{

}
