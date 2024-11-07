using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.AccessToken;
using TaskManager.Application.User.Common.Email.Code.Verifier;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.User.Commands.VerifyEmail;

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

        user.IsEmailVerified = true; // change

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);

        var accessToken = _accessTokenFactory.Create(user);

        return accessToken;
    }
}
