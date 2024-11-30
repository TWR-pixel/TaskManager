using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record LoginUserQuery(string EmailLogin, string Password) :
    QueryBase<AccessTokenResponse>;

public sealed class LoginUserQueryHandler(IReadonlyUnitOfWork unitOfWork,
                                          IPasswordHasher hasher,
                                          IAccessTokenFactory accessTokenFactory) :
    QueryHandlerBase<LoginUserQuery, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.GetByEmailAsync(request.EmailLogin, cancellationToken)
                   ?? throw new UserNotFoundException(request.EmailLogin); // get refresh token from db

        user.EmailConfirmed = true;

        if (!user.EmailConfirmed)
            throw new EmailNotVerifiedException($"User with email '{request.EmailLogin}' didn't confirmed email.");

        if (!hasher.Verify(request.Password, user.PasswordHash))
            throw new NotRightPasswordException(request.Password);

        var accessToken = accessTokenFactory.Create(user);

        return accessToken;
    }
}
