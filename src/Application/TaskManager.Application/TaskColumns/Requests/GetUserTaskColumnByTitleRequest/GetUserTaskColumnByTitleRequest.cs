using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumns.Requests.GetUserTaskColumnByTitleRequest;

public sealed record GetUserTaskColumnByTitleRequest : RequestBase<GetUserTaskColumnByTitleResponse>
{
}

public sealed record GetUserTaskColumnByTitleResponse : ResponseBase
{
}

public sealed class GetUserTaskColumnByTitleRequestHandler
    : RequestHandlerBase<GetUserTaskColumnByTitleRequest, GetUserTaskColumnByTitleResponse>
{
    public GetUserTaskColumnByTitleRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override Task<GetUserTaskColumnByTitleResponse> Handle(GetUserTaskColumnByTitleRequest request, CancellationToken cancellationToken)
    {

    }
}
