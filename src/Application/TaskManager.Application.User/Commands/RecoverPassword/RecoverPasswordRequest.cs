using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.User.Common.Email.Code.Verifier;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Users.Specifications;

namespace TaskManager.Application.User.Commands.RecoverPassword;

public sealed record RecoverPasswordRequest : CommandRequestBase<RecoverPasswordResponse>
{
    public required string Code { get; set; }
    public required string NewPassword { get; set; }
}

public sealed record RecoverPasswordResponse : ResponseBase
{
}

public sealed class RecoverPasswordRequestHandler : CommandRequestHandlerBase<RecoverPasswordRequest, RecoverPasswordResponse>
{
    private readonly ICodeVerifier _verifier;
    private readonly IPasswordHasher _hasher;

    public RecoverPasswordRequestHandler(IUnitOfWork unitOfWork,
                                         ICodeVerifier verifier,
                                         IPasswordHasher hasher) : base(unitOfWork)
    {
        _verifier = verifier;
        _hasher = hasher;
    }

    public override async Task<RecoverPasswordResponse> Handle(RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
        var isVerified = _verifier.Verify(request.Code, out string email);

        if (!isVerified)
            throw new CodeNotVerifiedException(email);

        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new GetUserByEmailSpec(email), cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (!user.IsEmailVerified)
            throw new EmailNotVerifiedException(user.EmailLogin);

        var salt = _hasher.GenerateSalt();
        var passwordHash = _hasher.HashPassword(request.NewPassword, salt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);

        var response = new RecoverPasswordResponse();

        return response;
    }
}
