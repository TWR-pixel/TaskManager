using TaskManager.Application.Common.Requests;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.ResendCode;

public sealed record ResendCodeRequest : RequestBase<ResendCodeResponse>
{
}

public sealed record ResendCodeResponse : ResponseBase
{
}

public sealed class ResendCodeRequestHandler : RequestHandlerBase<ResendCodeRequest, ResendCodeResponse>
{
    public ResendCodeRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
