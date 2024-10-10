using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Common.Security.Authentication.JwtClaims;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.GetNewUserAccessToken;

public sealed class GetNewUserAccessTokenRequest : RequestBase<GetNewUserAccessTokenResponse>
{
    public required string RefreshToken { get; set; }
}

public sealed class GetNewUserAccessTokenResponse : ResponseBase
{
    public required string NewAccessToken { get; set; }
}

public sealed class GetNewUserAccessTokenRequestHandler : RequestHandlerBase<GetNewUserAccessTokenRequest, GetNewUserAccessTokenResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo;
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _jwtClaimsFactory;

    public GetNewUserAccessTokenRequestHandler(EfRepositoryBase<UserEntity> usersRepo, IJwtSecurityTokenFactory jwtSecurityTokenFactory, IJwtClaimsFactory jwtClaimsFactory)
    {
        _usersRepo = usersRepo;
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _jwtClaimsFactory = jwtClaimsFactory;
    }

    public override async Task<GetNewUserAccessTokenResponse> Handle(GetNewUserAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _usersRepo
            .SingleOrDefaultAsync(new GetUserByRefreshToken(request.RefreshToken), cancellationToken);
        
        if (queryResult is null)
            throw new NullReferenceException(nameof(queryResult));

        var claims = _jwtClaimsFactory.CreateDefault(queryResult.Id, queryResult.Role.Id, queryResult.Username, queryResult.Role.Name);

        var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(_jwtSecurityTokenFactory.CreateSecurityToken(claims));

        return new GetNewUserAccessTokenResponse() { NewAccessToken = newAccessTokenString };
    }
}