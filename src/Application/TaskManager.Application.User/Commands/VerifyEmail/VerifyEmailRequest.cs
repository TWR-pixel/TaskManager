using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.AccessToken;
using TaskManager.Application.User.Common.Email.Code.Verifier;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands.VerifyEmail;

public sealed record VerifyEmailRequest(string Code) : RequestBase<AccessTokenResponse>;

public sealed class VerifyEmailRequestHandler(IUnitOfWork unitOfWork,
                                              ICodeVerifier verifier,
                                              IAccessTokenFactory accessTokenFactory)
        : RequestHandlerBase<VerifyEmailRequest, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var isCodeCorrect = verifier.Verify(request.Code, out string email);

        if (!isCodeCorrect)
            throw new CodeNotVerifiedException("code not found, try resend it");

        var user = await UnitOfWork.Users.GetByEmailAsync(email, cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (user.IsEmailVerified)
            throw new UserAlreadyVerifiedException(email);

        user.IsEmailVerified = true; // change

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var accessToken = accessTokenFactory.Create(user);

        return accessToken;
    }
}
