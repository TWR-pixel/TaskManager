using TaskManager.Application.Common.Requests;
using TaskManager.Application.User.Common.Email.Code;
using TaskManager.Application.User.Common.Security.AccessToken;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands.VerifyEmail;

public sealed record VerifyEmailRequest(string Code) : RequestBase<AccessTokenResponse>;

public sealed class VerifyEmailRequestHandler(IUnitOfWork unitOfWork,
                                              ICodeVerifier verifier,
                                              ICodeStorage codeStorage,
                                              IAccessTokenFactory accessTokenFactory)
        : RequestHandlerBase<VerifyEmailRequest, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var email = codeStorage.GetEmail(request.Code);
        if (email is null)
            throw new EmailNotVerifiedException(nameof(email));

        var isCodeCorrect = verifier.Verify(request.Code, email);
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
