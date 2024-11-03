using TaskManager.Application.Common.Security.Auth.AccessToken;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.Login;

public sealed record LoginUserRequest(string EmailLogin, string Password) :
    RequestBase<AccessTokenResponse>;

public sealed class LoginUserRequestHandler :
    RequestHandlerBase<LoginUserRequest, AccessTokenResponse>
{
    private readonly IPasswordHasher _hasher;
    private readonly IAccessTokenFactory _accessTokenFactory;

    public LoginUserRequestHandler(IUnitOfWork unitOfWork,
                                   IPasswordHasher hasher,
                                   IAccessTokenFactory accessTokenFactory) : base(unitOfWork)
    {
        _hasher = hasher;
        _accessTokenFactory = accessTokenFactory;
    }

    public override async Task<AccessTokenResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new GetUserByEmailSpec(request.EmailLogin), cancellationToken)
                          ?? throw new UserNotFoundException(request.EmailLogin); // get refresh token from db

        if (!user.IsEmailVerified)
            throw new EmailNotVerifiedException($"User with email '{request.EmailLogin}' didn't confirmed email.");

        if (!_hasher.Verify(request.Password, user.PasswordHash))
            throw new NotRightPasswordException(request.Password);

        var accessToken = _accessTokenFactory.Create(user);

        return accessToken;
    }
}
