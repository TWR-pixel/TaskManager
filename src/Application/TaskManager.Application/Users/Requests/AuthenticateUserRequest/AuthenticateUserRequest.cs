using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.Abstractions;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.Options;

namespace TaskManager.Application.Users.Requests.AuthenticateUserRequest;

public sealed class AuthenticateUserRequest :
    RequestBase<AuthenticateUserResponse>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
public sealed class AuthenticateUserResponse : ResponseBase
{
    public required JwtSecurityToken JwtToken { get; set; }
}

public sealed class AuthenticateUserRequestHandler :
    RequestHandlerBase<AuthenticateUserRequest, AuthenticateUserResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IOptions<JwtAuthenticationOptions> _options;

    public AuthenticateUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
        IOptions<JwtAuthenticationOptions> options)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _options = options;
    }

    public override async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new(nameof(request.UserName), request.UserName),
            new(nameof(request.Password), request.Password) // use hashing password
        };

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);
        var response = new AuthenticateUserResponse()
        {
            JwtToken = token
        };

        return response;
    }
}
