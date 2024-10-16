using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Common.Security.Authentication.JwtClaims;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.AuthenticateUserRequest;

public sealed record AuthenticateUserRequest(string EmailLogin, string Password) :
    RequestBase<AuthenticateUserResponse>;

public sealed record AuthenticateUserResponse : ResponseBase
{
    [SetsRequiredMembers]
    public AuthenticateUserResponse(string accessTokenString,
                                    string refreshTokenString,
                                    int userId,
                                    string userName,
                                    int roleId,
                                    string roleName)
    {
        AccessTokenString = accessTokenString;
        RefreshTokenString = refreshTokenString;
        UserId = userId;
        UserName = userName;
        RoleId = roleId;
        RoleName = roleName;
    }

    public required string AccessTokenString { get; set; }
    public required string RefreshTokenString { get; set; }

    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required int RoleId { get; set; }
    public required string RoleName { get; set; }
}

public sealed class AuthenticateUserRequestHandler :
    RequestHandlerBase<AuthenticateUserRequest, AuthenticateUserResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _claimsFactory;

    public AuthenticateUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                          IUnitOfWork unitOfWork,
                                          IJwtClaimsFactory claimsFactory) : base(unitOfWork)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _claimsFactory = claimsFactory;
    }

    public override async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpecification(request.EmailLogin), cancellationToken)
                          ?? throw new EntityNotFoundException($"User not found with email '{request.EmailLogin}', try register. "); // get refresh token from db


        var claims = _claimsFactory.CreateDefault(queryResult.Id,
                                                  queryResult.Role.Id,
                                                  queryResult.Username,
                                                  queryResult.Role.Name);

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);

        var response = new AuthenticateUserResponse(new JwtSecurityTokenHandler().WriteToken(token),
                                                    queryResult.RefreshToken,
                                                    queryResult.Id,
                                                    queryResult.Username,
                                                    queryResult.Role.Id,
                                                    queryResult.Role.Name);

        return response;
    }
}
