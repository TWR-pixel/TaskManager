using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Email;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record SendPasswordRecoveryCodeQuery(string Email) : QueryBase<SendPasswordRecoveryCodeResponse>;
public sealed record SendPasswordRecoveryCodeResponse : ResponseBase;

public sealed class SendPasswordRecoveryCodeQueryHandler(IReadonlyUnitOfWork unitOfWork,
                                     IEmailSender emailSender,
                                     ICodeGenerator<string> codeGenerator) : QueryHandlerBase<SendPasswordRecoveryCodeQuery, SendPasswordRecoveryCodeResponse>(unitOfWork)
{
    public override async Task<SendPasswordRecoveryCodeResponse> Handle(SendPasswordRecoveryCodeQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        var randomPasswordRecoveryCode = codeGenerator.GenerateCode(20);
        await emailSender.SendRecoveryPasswordMessageAsync(userEntity.EmailLogin, randomPasswordRecoveryCode, cancellationToken);

        return new SendPasswordRecoveryCodeResponse();
    }
}
