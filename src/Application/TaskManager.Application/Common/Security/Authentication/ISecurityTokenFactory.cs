using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Authentication;

/// <summary>
/// Generates new tokens for JWT authentication
/// </summary>
public interface ISecurityTokenFactory<TSecurityToken> where TSecurityToken : SecurityToken
{

}
