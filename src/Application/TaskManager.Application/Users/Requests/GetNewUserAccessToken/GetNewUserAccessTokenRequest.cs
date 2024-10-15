using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Common.Security.Authentication.JwtClaims;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.Users.Specifications;

namespace TaskManager.Application.Users.Requests.GetNewUserAccessToken;

public sealed record GetNewUserAccessTokenRequest : RequestBase<GetNewUserAccessTokenResponse>
{
    public required string RefreshToken { get; set; }
}

public sealed record GetNewUserAccessTokenResponse : ResponseBase
{
    [SetsRequiredMembers]
    public GetNewUserAccessTokenResponse(string newAccessToken)
    {
        NewAccessToken = newAccessToken;
    }

    public required string NewAccessToken { get; set; }
}

public sealed class GetNewUserAccessTokenRequestHandler : RequestHandlerBase<GetNewUserAccessTokenRequest, GetNewUserAccessTokenResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _jwtClaimsFactory;

    public GetNewUserAccessTokenRequestHandler(IUnitOfWork unitOfWork,
                                               IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                               IJwtClaimsFactory jwtClaimsFactory) : base(unitOfWork)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _jwtClaimsFactory = jwtClaimsFactory;
    }

    public override async Task<GetNewUserAccessTokenResponse> Handle(GetNewUserAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new GetUserByRefreshToken(request.RefreshToken), cancellationToken);

        if (queryResult is null)
            throw new NullReferenceException(nameof(queryResult));

        var claims = _jwtClaimsFactory.CreateDefault(queryResult.Id, queryResult.Role.Id, queryResult.Username, queryResult.Role.Name);

        var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(_jwtSecurityTokenFactory.CreateSecurityToken(claims));

        return new GetNewUserAccessTokenResponse(newAccessTokenString);
    }
}