using TaskManager.Application.Common.Email.Sender;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.ResendCode;

public sealed record ResendCodeRequest(string Email) : RequestBase<ResendCodeResponse>;
public sealed record ResendCodeResponse : ResponseBase;

public sealed class ResendCodeRequestHandler(IUnitOfWork unitOfWork, IEmailSender emailSender)
    : RequestHandlerBase<ResendCodeRequest, ResendCodeResponse>(unitOfWork)
{
    private readonly IEmailSender _emailSender = emailSender;

    public override async Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(request.Email), cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        await _emailSender.SendVerificationCodeAsync(request.Email, cancellationToken);

        var response = new ResendCodeResponse() { Status = "Success. The Code has been resent to your email" };

        return response;
    }
}
