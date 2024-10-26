﻿using TaskManager.Application.Common.Requests;
using TaskManager.Application.Modules.Email.Sender;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.ResendCode;

public sealed record ResendCodeRequest(string Email) : RequestBase<ResendCodeResponse>;
public sealed record ResendCodeResponse : ResponseBase;

public sealed class ResendCodeRequestHandler(IUnitOfWork unitOfWork, IEmailSenderService emailSender)
    : RequestHandlerBase<ResendCodeRequest, ResendCodeResponse>(unitOfWork)
{
    private readonly IEmailSenderService _emailSender = emailSender;
    public override async Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new GetUserByEmailSpec(request.Email), cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        if (user.IsEmailVerified)
            throw new UserAlreadyVerifiedException();

        await _emailSender.SendVerificationCodeAsync(request.Email, cancellationToken);

        var response = new ResendCodeResponse() { Status = "Success. The Code has been resent to your email" };

        return response;
    }
}