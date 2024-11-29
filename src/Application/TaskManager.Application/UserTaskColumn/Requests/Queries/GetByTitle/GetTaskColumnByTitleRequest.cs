using TaskManager.Application.Common.Requests;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTaskColumn.Requests.Queries.GetByTitle;

public sealed record GetTaskColumnByTitleRequest : RequestBase<GetTaskColumnByTitleResponse>
{
}

public sealed record GetTaskColumnByTitleResponse : ResponseBase
{
}

public sealed class GetTaskColumnByTitleRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<GetTaskColumnByTitleRequest, GetTaskColumnByTitleResponse>(unitOfWork)
{
    public override Task<GetTaskColumnByTitleResponse> Handle(GetTaskColumnByTitleRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
