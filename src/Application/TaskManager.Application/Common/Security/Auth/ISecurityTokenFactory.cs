using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Common.Security.Auth;

/// <summary>
/// Generates new tokens for JWT authentication
/// </summary>
public interface ISecurityTokenFactory<TSecurityToken> where TSecurityToken : SecurityToken
{

}
