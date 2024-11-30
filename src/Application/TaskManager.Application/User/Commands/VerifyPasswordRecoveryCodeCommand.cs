using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.Common.Security;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed record VerifyPasswordRecoveryCodeCommand : CommandBase<VerifyPasswordRecoveryCodeResponse>
{
    public required string Code { get; set; }
    public required string NewPassword { get; set; }
}

public sealed record VerifyPasswordRecoveryCodeResponse : ResponseBase;

public sealed class VerifyPasswordRecoveryCodeCommandHandler(IUnitOfWork unitOfWork,
                                     ICodeVerifier verifier,
                                     ICodeStorage codeStorage,
                                     IPasswordHasher hasher) : CommandHandlerBase<VerifyPasswordRecoveryCodeCommand, VerifyPasswordRecoveryCodeResponse>(unitOfWork)
{
    public override async Task<VerifyPasswordRecoveryCodeResponse> Handle(VerifyPasswordRecoveryCodeCommand request, CancellationToken cancellationToken)
    {
        var email = codeStorage.GetEmail(request.Code);
        if (email is null)
            throw new EmailNotVerifiedException(nameof(email));

        var isVerified = verifier.Verify(request.Code, email);
        if (!isVerified)
            throw new CodeNotVerifiedException(email);

        codeStorage.Remove(request.Code);

        var user = await UnitOfWork.Users.GetByEmailAsync(email, cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (!user.EmailConfirmed)
            throw new EmailNotVerifiedException(user.EmailLogin);

        var salt = hasher.GenerateSalt();
        var passwordHash = hasher.HashPassword(request.NewPassword, salt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = new VerifyPasswordRecoveryCodeResponse();

        return response;
    }
}
