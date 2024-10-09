using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Common.Security.Authentication.JwtClaims;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.AuthenticateUserRequest;

public sealed class AuthenticateUserRequest :
    RequestBase<AuthenticateUserResponse>
{
    public required string EmailLogin { get; set; }
    public required string Password { get; set; }
}
public sealed class AuthenticateUserResponse : ResponseBase
{
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
    private readonly IMemoryCache _memoryCache;

    private readonly EfRepositoryBase<UserEntity> _userRepo;

    public AuthenticateUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                          EfRepositoryBase<UserEntity> userRepo,
                                          IJwtClaimsFactory claimsFactory,
                                          IMemoryCache memoryCache)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _userRepo = userRepo;
        _claimsFactory = claimsFactory;
        _memoryCache = memoryCache;
    }

    public override async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _userRepo.SingleOrDefaultAsync(new GetUserByEmailLoginWithRoleSpecification(request.EmailLogin), cancellationToken)
                          ?? throw new EntityNotFoundException("User not found by id. Try register new user."); // get refresh token from db

        
        var claims = _claimsFactory.CreateDefault(queryResult.Id,
                                                  queryResult.Role.Id,
                                                  queryResult.Username,
                                                  queryResult.Role.Name);

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);

        var response = new AuthenticateUserResponse()
        {
            AccessTokenString = new JwtSecurityTokenHandler().WriteToken(token),
            RoleId = queryResult.Role.Id,
            RoleName = queryResult.Role.Name,
            UserId = queryResult.Id,
            UserName = queryResult.Username,
            RefreshTokenString = queryResult.RefreshToken
        };

        return response;
    }
}
