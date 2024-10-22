using Microsoft.Extensions.Options;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Modules.Email.Sender;
using TaskManager.Application.Modules.Email.Sender.Options;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.ResendCode;

public sealed record ResendCodeRequest(string Email) : RequestBase<ResendCodeResponse>;
public sealed record ResendCodeResponse : ResponseBase;

public sealed class ResendCodeRequestHandler(IUnitOfWork unitOfWork, IEmailSender emailSender, IOptions<EmailSenderOptions> emailOptions)
    : RequestHandlerBase<ResendCodeRequest, ResendCodeResponse>(unitOfWork)
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly EmailSenderOptions _emailOptions = emailOptions.Value;

    public override async Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(request.Email), cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        await _emailSender.SendVerificationCodeAsync(_emailOptions.From, request.Email, cancellationToken);

        var response = new ResendCodeResponse() { Status = "Success. The Code has been resent to your email" };

        return response;
    }
}
