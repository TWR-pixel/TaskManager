using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Application.Tasks.Requests.GetAllUsersTasksById;

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
