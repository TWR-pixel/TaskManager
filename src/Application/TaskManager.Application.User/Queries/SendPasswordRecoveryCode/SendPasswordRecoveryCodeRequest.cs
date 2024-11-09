using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.Email.Sender;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Users.Specifications;

namespace TaskManager.Application.User.Queries.SendPasswordRecoveryCode;

public sealed record SendPasswordRecoveryCodeRequest(string Email) : RequestBase<SendPasswordRecoveryCodeResponse>;
public sealed record SendPasswordRecoveryCodeResponse : ResponseBase;

public sealed class SendPasswordRecoveryCodeRequestHandler : RequestHandlerBase<SendPasswordRecoveryCodeRequest, SendPasswordRecoveryCodeResponse>
{
    private readonly IEmailSenderService _emailSender;

    public SendPasswordRecoveryCodeRequestHandler(IUnitOfWork unitOfWork,
                                         IEmailSenderService emailSender) : base(unitOfWork)
    {
        _emailSender = emailSender;
    }

    public override async Task<SendPasswordRecoveryCodeResponse> Handle(SendPasswordRecoveryCodeRequest request, CancellationToken cancellationToken)
    {
        _ = await UnitOfWork.Users.SingleOrDefaultAsync(new GetUserByEmailSpec(request.Email), cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        await _emailSender.SendRecoveryPasswordMessageAsync(request.Email, cancellationToken);

        return new SendPasswordRecoveryCodeResponse();
    }
}
