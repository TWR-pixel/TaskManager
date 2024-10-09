using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Common.Security.Authentication.JwtClaims;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Users.Requests.GetUserAccessTokenRequest;

public sealed class GetUserAccessTokenRequest : RequestBase<GetUserAccessTokenResponse>
{
    public int UserId { get; set; }
}

public sealed class GetUserAccessTokenResponse : ResponseBase
{
    public required string AccessTokenString { get; set; }
}

public sealed class GetUserAccessTokenRequestHandler : RequestHandlerBase<GetUserAccessTokenRequest, GetUserAccessTokenResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo;
    private readonly IMemoryCache _memCache;
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _claimsFactory;

    public GetUserAccessTokenRequestHandler(EfRepositoryBase<UserEntity> usersRepo,
                                               IMemoryCache memCache,
                                               IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                               IJwtClaimsFactory claimsFactory)
    {
        _usersRepo = usersRepo;
        _memCache = memCache;
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _claimsFactory = claimsFactory;
    }

    public override async Task<GetUserAccessTokenResponse> Handle(GetUserAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _usersRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        var refreshTokenString = "";
        _memCache.TryGetValue(queryResult.Id.ToString(), out refreshTokenString);

        if (refreshTokenString == null)
        {
            return null;
        }

        var claims = _claimsFactory.CreateDefault(queryResult.Id,
                                          queryResult.Role.Id,
                                          queryResult.Username,
                                          queryResult.Role.Name);

        var newAccessToken = _jwtSecurityTokenFactory.CreateSecurityToken(claims);

        return new GetUserAccessTokenResponse
        {
            AccessTokenString = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
        };
    }
}
