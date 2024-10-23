using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Application.Modules.Email.Verifier;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.RecoverPassword;

public sealed record RecoverPasswordRequest : RequestBase<RecoverPasswordResponse>
{
    public required string Code { get; set; }
    public required string NewPassword { get; set; }
}

public sealed record RecoverPasswordResponse : ResponseBase
{
}

public sealed class RecoverPasswordRequestHandler : RequestHandlerBase<RecoverPasswordRequest, RecoverPasswordResponse>
{
    private readonly ICodeVerifier _verifier;
    private readonly IPasswordHasher _hasher;

    public RecoverPasswordRequestHandler(IUnitOfWork unitOfWork, ICodeVerifier verifier, IPasswordHasher hasher) : base(unitOfWork)
    {
        _verifier = verifier;
        _hasher = hasher;
    }

    public override async Task<RecoverPasswordResponse> Handle(RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
        var isVerified = _verifier.Verify(request.Code, out string email);

        if (!isVerified)
            throw new CodeNotVerifiedException(email);

        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(email), cancellationToken)
            ?? throw new UserNotFoundException(email);

        if (!user.IsEmailVerified)
            throw new EmailNotVerifiedException();

        var salt = _hasher.GenerateSalt();
        var passwordHash = _hasher.HashPassword(request.NewPassword, salt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;

        await UnitOfWork.Users.UpdateAsync(user, cancellationToken);

        var response = new RecoverPasswordResponse();

        return response;
    }
}
