using TaskManager.Application.Common.Email.Sender;
using TaskManager.Application.Common.Requests;
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


    public RecoverPasswordRequestHandler(IUnitOfWork unitOfWork, IEmailSender emailSender) : base(unitOfWork)
    {
        _emailSender = emailSender;
    }

    public override async Task<RecoverPasswordResponse> Handle(RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
        await _emailSender.SendVerificationCodeAsync(request.Email, cancellationToken);
        


        throw new NotImplementedException();
    }
}
