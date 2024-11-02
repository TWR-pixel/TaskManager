using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Security.Auth.Claims;
using TaskManager.Application.Common.Security.Auth.Tokens.Jwt;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.Common.Security.Auth.AccessToken;

public sealed class AccessTokenFactory : IAccessTokenFactory
{
    private readonly IJwtSecurityTokenFactory _tokenFactory;
    private readonly IClaimsFactory _claimsFactory;

    public AccessTokenFactory(IJwtSecurityTokenFactory tokenFactory, IClaimsFactory claimsFactory)
    {
        _tokenFactory = tokenFactory;
        _claimsFactory = claimsFactory;
    }

    public AccessTokenResponse Create(UserEntity user)
    {
        if (user.Role is null)
            throw new NullReferenceException(nameof(user.Role));

        var claims = _claimsFactory.Create(user.Id, user.Role.Id, user.Username, user.Role.Name);
        var token = _tokenFactory.Create(claims);

        var accessToken = new AccessTokenResponse(new JwtSecurityTokenHandler().WriteToken(token),
                                                  user.Id,
                                                  user.Username,
                                                  user.Role.Id,
                                                  user.Role.Name);

        return accessToken;
    }
}
