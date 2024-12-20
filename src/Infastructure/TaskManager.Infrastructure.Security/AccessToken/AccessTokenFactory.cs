﻿using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth;
using TaskManager.Application.Common.Security.Auth.Jwt;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Infrastructure.Security.AccessToken;

public sealed class AccessTokenFactory : IAccessTokenFactory
{
    private readonly IJwtTokenFactory _tokenFactory;
    private readonly IClaimsFactory _claimsFactory;

    public AccessTokenFactory(IJwtTokenFactory tokenFactory, IClaimsFactory claimsFactory)
    {
        _tokenFactory = tokenFactory;
        _claimsFactory = claimsFactory;
    }

    public AccessTokenResponse Create(UserEntity user)
    {
        if (user.Role is null)
            throw new NullReferenceException(nameof(user.Role));

        var claims = _claimsFactory.Create(user.Id, user.Role.Id, user.UserName, user.Role.Name);
        var token = _tokenFactory.Create(claims);

        var accessToken = new AccessTokenResponse(new JwtSecurityTokenHandler().WriteToken(token),
                                                  user.Id,
                                                  user.UserName,
                                                  user.Role.Id,
                                                  user.Role.Name);

        return accessToken;
    }
}
