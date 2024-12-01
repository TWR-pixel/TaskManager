using Microsoft.AspNetCore.Identity;
using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth.OAuth.Google;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record LoginUserWithJwtBearerSchemeQuery(string EmailLogin, string Password) :
    QueryBase<AccessTokenResponse>;

public sealed class LoginUserWithJwtBearerSchemeQueryHandler(IReadonlyUnitOfWork unitOfWork,
                                          IPasswordHasher hasher,
                                          IAccessTokenFactory accessTokenFactory,
                                          UserManager<UserEntity> userManager) :
    QueryHandlerBase<LoginUserWithJwtBearerSchemeQuery, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(LoginUserWithJwtBearerSchemeQuery request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.GetByEmailAsync(request.EmailLogin, cancellationToken)
                   ?? throw new UserNotFoundException(request.EmailLogin);

        if (user.AuthenticationScheme == GoogleOAuthDefaults.AuthenticationScheme)
            throw new GoogleOAuthRegisteredException(user.Email!);

        user.EmailConfirmed = true;
        if (!user.EmailConfirmed)
            throw new EmailNotVerifiedException($"User with email '{request.EmailLogin}' didn't confirmed email.");

        var accessToken = accessTokenFactory.Create(user);

        if (user.PasswordHash is null)
            return accessToken;

        if (!hasher.Verify(request.Password, user.PasswordHash))
            throw new NotRightPasswordException(request.Password);

        return accessToken;
    }
}
