﻿using TaskManager.Application.Common.Requests;
using TaskManager.Application.Modules.Email.Sender;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.SendPasswordRecoveryCode;

public sealed record SendPasswordRecoveryCodeRequest(string Email) : RequestBase<SendPasswordRecoveryCodeResponse>;
public sealed record SendPasswordRecoveryCodeResponse : ResponseBase;

public sealed class SendPasswordRecoveryCodeRequestHandler : RequestHandlerBase<SendPasswordRecoveryCodeRequest, SendPasswordRecoveryCodeResponse>
{
    private readonly IEmailSender _emailSender;

    public SendPasswordRecoveryCodeRequestHandler(IUnitOfWork unitOfWork,
                                         IEmailSender emailSender) : base(unitOfWork)
    {
        _emailSender = emailSender;
    }

    public override async Task<SendPasswordRecoveryCodeResponse> Handle(SendPasswordRecoveryCodeRequest request, CancellationToken cancellationToken)
    {
        _ = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(request.Email), cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        await _emailSender.SendRecoveryCodeAsync(request.Email, cancellationToken);

        return new SendPasswordRecoveryCodeResponse();
    }
}
