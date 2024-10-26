using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Auth.Claims;
using TaskManager.Application.Common.Security.Auth.Tokens;
using TaskManager.Application.Common.Security.Hashers;
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
    private readonly IClaimsFactory _claimsFactory;
    private readonly IPasswordHasher _hasher;

    public LoginUserRequestHandler(IJwtSecurityTokenFactory jwtSecurityTokenFactory,
                                          IUnitOfWork unitOfWork,
                                          IClaimsFactory claimsFactory,
                                          IPasswordHasher hasher) : base(unitOfWork)
    {
        _jwtSecurityTokenFactory = jwtSecurityTokenFactory;
        _claimsFactory = claimsFactory;
        _hasher = hasher;
    }

    public override async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new GetUserByEmailSpec(request.EmailLogin), cancellationToken)
                          ?? throw new UserNotFoundException(request.EmailLogin); // get refresh token from db

        if (!user.IsEmailVerified)
            throw new EmailNotVerifiedException($"User with email '{request.EmailLogin}' didn't confirmed email.");

        if (!_hasher.Verify(request.Password, user.PasswordHash))
            throw new NotRightPasswordException(request.Password);


        var claims = _claimsFactory.Create(user.Id,
                                           user.Role.Id,
                                           user.Username,
                                           user.Role.Name);

        var token = _jwtSecurityTokenFactory.Create(claims);

        var response = new LoginUserResponse(new JwtSecurityTokenHandler().WriteToken(token),
                                                    user.Id,
                                                    user.Username,
                                                    user.Role.Id,
                                                    user.Role.Name);

        return response;
    }
}
