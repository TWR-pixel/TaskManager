using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.Email.Sender;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.ResendCode;

public sealed record ResendCodeRequest(string Email) : RequestBase<ResendCodeResponse>;
public sealed record ResendCodeResponse : ResponseBase;

public sealed class ResendCodeRequestHandler(IUnitOfWork unitOfWork, IEmailSenderService emailSender)
    : RequestHandlerBase<ResendCodeRequest, ResendCodeResponse>(unitOfWork)
{
    private readonly IEmailSenderService _emailSender = emailSender;

    public override async Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        if (user.IsEmailVerified)
            throw new UserAlreadyVerifiedException();

        await _emailSender.SendVerificationMessageAsync(request.Email, cancellationToken);

        var response = new ResendCodeResponse() { Status = "Success. The Code has been resent to your email" };

        return response;
    }
}
