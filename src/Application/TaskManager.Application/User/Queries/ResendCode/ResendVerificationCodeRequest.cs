using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Email;
using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.ResendCode;

public sealed record ResendVerificationCodeRequest(string Email) : RequestBase<ResendVerificationCodeResponse>;
public sealed record ResendVerificationCodeResponse : ResponseBase;

public sealed class ResendVerificationCodeRequestHandler(IUnitOfWork unitOfWork, IEmailSender emailSender, ICodeGenerator<string> codeGenerator)
    : RequestHandlerBase<ResendVerificationCodeRequest, ResendVerificationCodeResponse>(unitOfWork)
{
    private readonly IEmailSender _emailSender = emailSender;

    public override async Task<ResendVerificationCodeResponse> Handle(ResendVerificationCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new UserNotFoundException(request.Email);

        if (user.IsEmailVerified)
            throw new UserAlreadyVerifiedException();

        var randomEmailVerificationCode = codeGenerator.GenerateCode(20);
        await _emailSender.SendVerificationMessageAsync(request.Email, randomEmailVerificationCode, cancellationToken);

        var response = new ResendVerificationCodeResponse() { Status = "Success. The Code has been resent to your email" };

        return response;
    }
}
