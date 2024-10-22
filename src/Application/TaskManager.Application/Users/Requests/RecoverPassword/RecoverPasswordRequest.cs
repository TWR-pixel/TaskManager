using Microsoft.Extensions.Options;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Modules.Email.Sender;
using TaskManager.Application.Modules.Email.Sender.Options;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.RecoverPassword;

public sealed record RecoverPasswordRequest(string Email) : RequestBase<RecoverPasswordResponse>
{
}

public sealed record RecoverPasswordResponse : ResponseBase
{
}

public sealed class RecoverPasswordRequestHandler : RequestHandlerBase<RecoverPasswordRequest, RecoverPasswordResponse>
{
    private readonly IEmailSender _emailSender;
    private readonly EmailSenderOptions _emailOptions;

    public RecoverPasswordRequestHandler(IUnitOfWork unitOfWork, IEmailSender emailSender, IOptions<EmailSenderOptions> emailOptions) : base(unitOfWork)
    {
        _emailSender = emailSender;
        _emailOptions = emailOptions.Value;
    }

    public override async Task<RecoverPasswordResponse> Handle(RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
        await _emailSender.SendVerificationCodeAsync(_emailOptions.From, request.Email, cancellationToken);



        throw new NotImplementedException();
    }
}
