using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.Login;

public sealed record LoginUserRequest(string EmailLogin, string Password) :
    RequestBase<LoginUserResponse>;

public sealed record LoginUserResponse(string AccessToken,
                                              int UserId,
                                              string Username,
                                              int RoleId,
                                              string RoleName) : ResponseBase;

public sealed class LoginUserRequestHandler :
    RequestHandlerBase<LoginUserRequest, LoginUserResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtSecurityTokenFactory;
    private readonly IJwtClaimsFactory _claimsFactory;

    public LoginUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                          IUnitOfWork unitOfWork,
                                          IJwtClaimsFactory claimsFactory) : base(unitOfWork)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _claimsFactory = claimsFactory;
    }

    public override async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(request.EmailLogin), cancellationToken)
                          ?? throw new UserNotFoundException(request.EmailLogin); // get refresh token from db
        
        if (!queryResult.IsEmailConfirmed)
            throw new EmailNotConfirmedException($"User with email '{request.EmailLogin}' didn't confirmed email.");
        
        var claims = _claimsFactory.CreateDefault(queryResult.Id,
                                                  queryResult.Role.Id,
                                                  queryResult.Username,
                                                  queryResult.Role.Name);

        var token = _jwtSecurityTokenFactory.CreateSecurityToken(claims);

        var response = new LoginUserResponse(new JwtSecurityTokenHandler().WriteToken(token),
                                                    queryResult.Id,
                                                    queryResult.Username,
                                                    queryResult.Role.Id,
                                                    queryResult.Role.Name);

        return response;
    }
}
