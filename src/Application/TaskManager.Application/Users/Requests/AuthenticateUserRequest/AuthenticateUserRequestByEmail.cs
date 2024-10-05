using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication;
using TaskManager.Application.Common.Security.Authentication.Abstractions;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.AuthenticateUserRequest;

public sealed class AuthenticateUserRequestByEmail :
    RequestBase<AuthenticateUserByEmailResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
public sealed class AuthenticateUserByEmailResponse : ResponseBase
{
    public required string TokenString { get; set; }
}

public sealed class AuthenticateUserRequestHandler :
    RequestHandlerBase<AuthenticateUserRequestByEmail, AuthenticateUserByEmailResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _claimsFactory;

    private readonly EfRepositoryBase<UserEntity> _userRepo;

    public AuthenticateUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                          EfRepositoryBase<UserEntity> userRepo,
                                          IJwtClaimsFactory claimsFactory)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _userRepo = userRepo;
        _claimsFactory = claimsFactory;
    }

    public override async Task<AuthenticateUserByEmailResponse> Handle(AuthenticateUserRequestByEmail request, CancellationToken cancellationToken)
    {
        var queryResult = await _userRepo.SingleOrDefaultAsync(new GetUserByEmailLoginSpecification(request.Email), cancellationToken)
                          ?? throw new EntityNotFoundException("User not found by id. Try register new user.");

        var claims = _claimsFactory.CreateDefault(queryResult.Id,
                                                  queryResult.Role.Id,
                                                  queryResult.Username,
                                                  queryResult.Role.Name);

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);

        var response = new AuthenticateUserByEmailResponse()
        {
            TokenString = new JwtSecurityTokenHandler().WriteToken(token),
        };

        return response;
    }
}
