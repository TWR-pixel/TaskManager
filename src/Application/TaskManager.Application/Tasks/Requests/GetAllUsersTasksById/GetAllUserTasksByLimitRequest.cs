using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Data;

namespace TaskManager.Application.Tasks.Requests.GetAllUsersTasksById;

public sealed class GetAllUserTasksByLimitRequest : RequestBase<GetAllUserTasksByLimitResponse>
{
}

public sealed class GetAllUserTasksByLimitResponse : ResponseBase
{
}

public sealed class GetAllUserTasksByLimitRequestHandler(EfRepositoryBase<TaskEntity> userTasksRepo) : RequestHandlerBase<GetAllUserTasksByLimitRequest, GetAllUserTasksByLimitResponse>
{
    private readonly EfRepositoryBase<TaskEntity> _userTasksRepo = userTasksRepo;

    public override async Task<GetAllUserTasksByLimitResponse> Handle(GetAllUserTasksByLimitRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _userTasksRepo.ListAsync(cancellationToken);

        throw new NotImplementedException();
    }
}
