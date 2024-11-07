using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.GetAllById;

public sealed record GetAllUserTasksByLimitRequest : RequestBase<GetAllUserTasksByLimitResponse>
{
}

public sealed record GetAllUserTasksByLimitResponse : ResponseBase
{
}

public sealed class GetAllUserTasksByLimitRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<GetAllUserTasksByLimitRequest, GetAllUserTasksByLimitResponse>(unitOfWork)
{
    public override async Task<GetAllUserTasksByLimitResponse> Handle(GetAllUserTasksByLimitRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.ListAsync(cancellationToken);

        throw new NotImplementedException();
    }
}
