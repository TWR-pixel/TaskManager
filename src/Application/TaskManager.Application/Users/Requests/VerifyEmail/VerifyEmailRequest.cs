using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.Common.Security.Auth.AccessToken;
using TaskManager.Application.Modules.Email.Code.Verifier;
using TaskManager.Application.Users.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.VerifyEmail;

public sealed record VerifyEmailRequest(string Code) : RequestBase<AccessTokenResponse>;

public sealed class VerifyEmailRequestHandler
    : RequestHandlerBase<VerifyEmailRequest, AccessTokenResponse>
{
    private readonly ICodeVerifier _verifier;
    private readonly IAccessTokenFactory _accessTokenFactory;


    public VerifyEmailRequestHandler(IUnitOfWork unitOfWork,
                                     ICodeVerifier verifier,
                                     IAccessTokenFactory accessTokenFactory) : base(unitOfWork)
    {
        _verifier = verifier;
        _accessTokenFactory = accessTokenFactory;
    }

    public override async Task<AccessTokenResponse> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var isCodeCorrect = _verifier.Verify(request.Code, out string email);

        if (!isCodeCorrect)
            throw new CodeNotVerifiedException("code not found, try resend it");

        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new GetUserByEmailSpec(email), cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (user.IsEmailVerified)
            throw new UserAlreadyVerifiedException(email);

        user.IsEmailVerified = true; // make it in interceptor ef core

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);

        var accessToken = _accessTokenFactory.Create(user);

        return accessToken;
    }
}
