using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Users.Requests.Identity.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.Users.Requests.Identity.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.GetNewAccessToken;

public sealed record GetNewAccessTokenRequest(string RefreshToken) : RequestBase<GetUserAccessTokenResponse>;
public sealed record GetUserAccessTokenResponse(string NewAccessToken) : ResponseBase;

public sealed class GetUserAccessTokenRequestHandler : RequestHandlerBase<GetNewAccessTokenRequest, GetUserAccessTokenResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _jwtClaimsFactory;

    public GetUserAccessTokenRequestHandler(IUnitOfWork unitOfWork,
                                               IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                               IJwtClaimsFactory jwtClaimsFactory) : base(unitOfWork)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _jwtClaimsFactory = jwtClaimsFactory;
    }

    public override async Task<GetUserAccessTokenResponse> Handle(GetNewAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadUserByRefreshToken(request.RefreshToken), cancellationToken)
                ?? throw new EntityNotFoundException($"User with refresh token '{request.RefreshToken}' not found");

        var claims = _jwtClaimsFactory.CreateDefault(queryResult.Id,
                                                     queryResult.Role.Id,
                                                     queryResult.Username,
                                                     queryResult.Role.Name);

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);

        var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new GetUserAccessTokenResponse(newAccessTokenString);
    }
}