using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Email;
using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.SendPasswordRecoveryCode;

public sealed record SendPasswordRecoveryCodeRequest(string Email) : RequestBase<SendPasswordRecoveryCodeResponse>;
public sealed record SendPasswordRecoveryCodeResponse : ResponseBase;

public sealed class SendPasswordRecoveryCodeRequestHandler(IUnitOfWork unitOfWork,
                                     IEmailSender emailSender,
                                     ICodeGenerator<string> codeGenerator) : RequestHandlerBase<SendPasswordRecoveryCodeRequest, SendPasswordRecoveryCodeResponse>(unitOfWork)
{
    public override async Task<SendPasswordRecoveryCodeResponse> Handle(SendPasswordRecoveryCodeRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        var randomPasswordRecoveryCode = codeGenerator.GenerateCode(20);
        await emailSender.SendRecoveryPasswordMessageAsync(userEntity.EmailLogin, randomPasswordRecoveryCode, cancellationToken);

        return new SendPasswordRecoveryCodeResponse();
    }
}
