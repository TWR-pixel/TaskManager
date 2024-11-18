using TaskManager.Application.Common.Requests;
using TaskManager.Application.User.Common.Email;
using TaskManager.Application.User.Common.Email.Code;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.SendPasswordRecoveryCode;

public sealed record SendRecoveryCodeRequest(string Email) : RequestBase<SendPasswordRecoveryCodeResponse>;
public sealed record SendPasswordRecoveryCodeResponse : ResponseBase;

public sealed class SendPasswordRecoveryCodeRequestHandler(IUnitOfWork unitOfWork,
                                     IEmailSender emailSender,
                                     ICodeGenerator<string> codeGenerator) : RequestHandlerBase<SendRecoveryCodeRequest, SendPasswordRecoveryCodeResponse>(unitOfWork)
{
    public override async Task<SendPasswordRecoveryCodeResponse> Handle(SendRecoveryCodeRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        await emailSender.SendRecoveryPasswordMessageAsync(userEntity.EmailLogin, codeGenerator.GenerateCode(20), cancellationToken);

        return new SendPasswordRecoveryCodeResponse();
    }
}
