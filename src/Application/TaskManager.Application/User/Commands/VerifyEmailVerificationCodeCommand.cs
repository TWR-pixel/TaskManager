using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed record VerifyEmailVerificationCodeCommand(string Code) : CommandBase<AccessTokenResponse>;

public sealed class VerifyEmailVerificationCodeCommandHandler(IUnitOfWork unitOfWork,
                                                              ICodeVerifier verifier,
                                                              ICodeStorage codeStorage,
                                                              IAccessTokenFactory accessTokenFactory)
        : CommandHandlerBase<VerifyEmailVerificationCodeCommand, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(VerifyEmailVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var email = codeStorage.GetEmail(request.Code);
        if (email is null)
            throw new EmailNotVerifiedException(nameof(email));

        var isCodeCorrect = verifier.Verify(request.Code, email);
        if (!isCodeCorrect)
            throw new CodeNotVerifiedException("code not found, try resend it");

        codeStorage.Remove(request.Code);

        var user = await UnitOfWork.Users.GetByEmailAsync(email, cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (user.EmailConfirmed)
            throw new UserAlreadyVerifiedException(email);
        user.EmailConfirmed = true; // change in production

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var accessToken = accessTokenFactory.Create(user);

        return accessToken;
    }
}
