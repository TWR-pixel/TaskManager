using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth.OAuth.Google;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record LoginUserQuery(string EmailLogin, string Password) :
    QueryBase<AccessTokenResponse>;

public sealed class LoginUserQueryHandler(IReadonlyUnitOfWork unitOfWork,
                                          IPasswordHasher hasher,
                                          IAccessTokenFactory accessTokenFactory) : QueryHandlerBase<LoginUserQuery, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.GetByEmailAsync(request.EmailLogin, cancellationToken)
                   ?? throw new UserNotFoundException(request.EmailLogin);

        if (user.AuthenticationScheme == GoogleOAuthDefaults.AuthenticationScheme)
            throw new GoogleOAuthRegisteredException(user.Email!);

        user.EmailConfirmed = true;
        if (!user.EmailConfirmed)
            throw new EmailNotVerifiedException($"User with email '{request.EmailLogin}' didn't confirmed email.");

        var accessToken = accessTokenFactory.Create(user);

        if (user.AuthenticationScheme == GoogleOAuthDefaults.AuthenticationScheme)
            return accessToken;

        var requestPasswordHash = hasher.HashPassword(request.Password, user.PasswordSalt);
        if (requestPasswordHash != user.PasswordHash)
            throw new NotRightPasswordException(request.Password);

        return accessToken;
    }
}
