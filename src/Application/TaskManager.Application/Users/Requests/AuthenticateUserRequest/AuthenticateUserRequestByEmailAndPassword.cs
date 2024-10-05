using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.Abstractions;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.Options;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.AuthenticateUserRequest;

public sealed class AuthenticateUserRequestByEmailAndPassword :
    RequestBase<AuthenticateUserResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
public sealed class AuthenticateUserResponse : ResponseBase
{
    public required string TokenString { get; set; }
}

public sealed class AuthenticateUserRequestHandler :
    RequestHandlerBase<AuthenticateUserRequestByEmailAndPassword, AuthenticateUserResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IOptions<JwtAuthenticationOptions> _options;
    private readonly EfRepositoryBase<UserEntity> _userRepo;

    public AuthenticateUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
        IOptions<JwtAuthenticationOptions> options,
        EfRepositoryBase<UserEntity> userRepo)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _options = options;
        _userRepo = userRepo;
    }

    public override async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequestByEmailAndPassword request, CancellationToken cancellationToken)
    {
        var queryResult = await _userRepo
            .SingleOrDefaultAsync(new GetUserEntityByEmailLoginSpecification(request.Email), cancellationToken);

        if (queryResult == null)
        {
            throw new EntityNotFoundException("User not found. Try register new user.");
        }

        var claims = new List<Claim>()
        {
            new("Id", queryResult.Id.ToString()),
            new(nameof(request.Password), request.Password) // use hashing password
        };

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);
        var response = new AuthenticateUserResponse()
        {
            TokenString = new JwtSecurityTokenHandler().WriteToken(token),
        };

        return response;
    }
}
