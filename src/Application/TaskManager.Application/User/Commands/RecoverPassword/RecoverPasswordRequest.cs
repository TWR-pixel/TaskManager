using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.User.Common.Email.Code;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands.RecoverPassword;

public sealed record RecoverPasswordRequest : CommandRequestBase<RecoverPasswordResponse>
{
    public required string Code { get; set; }
    public required string NewPassword { get; set; }
}

public sealed record RecoverPasswordResponse : ResponseBase
{
}

public sealed class RecoverPasswordRequestHandler(IUnitOfWork unitOfWork,
                                     ICodeVerifier verifier,
                                     ICodeStorage codeStorage,
                                     IPasswordHasher hasher) : CommandRequestHandlerBase<RecoverPasswordRequest, RecoverPasswordResponse>(unitOfWork)
{
    public override async Task<RecoverPasswordResponse> Handle(RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
        var email = codeStorage.GetEmail(request.Code);
        if (email is null)
            throw new EmailNotVerifiedException(nameof(email));

        var isVerified = verifier.Verify(request.Code, email);
        if (!isVerified)
            throw new CodeNotVerifiedException(email);

        var user = await UnitOfWork.Users.GetByEmailAsync(email, cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (!user.IsEmailVerified)
            throw new EmailNotVerifiedException(user.EmailLogin);

        var salt = hasher.GenerateSalt();
        var passwordHash = hasher.HashPassword(request.NewPassword, salt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        
        var response = new RecoverPasswordResponse();

        return response;
    }
}
