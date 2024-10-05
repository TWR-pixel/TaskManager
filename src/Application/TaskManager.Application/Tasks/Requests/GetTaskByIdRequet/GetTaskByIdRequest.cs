using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Data;

namespace TaskManager.Application.Tasks.Requests.GetTaskByIdRequet;

public sealed class GetTaskByIdRequest : RequestBase<GetTaskByIdResponse>
{
    public required int TaskId { get; set; }
}

public sealed class GetTaskByIdResponse : ResponseBase
{
}

public sealed class GetTaskByIdRequetHandler : RequestHandlerBase<GetTaskByIdRequest, GetTaskByIdResponse>
{
    private readonly EfRepositoryBase<TaskEntity> _tasksRepo;

    public GetTaskByIdRequetHandler(EfRepositoryBase<TaskEntity> tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    public override async Task<GetTaskByIdResponse> Handle(GetTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _tasksRepo.GetByIdAsync(request.TaskId)
            ?? throw new EntityNotFoundException("Task not found by id = " + request.TaskId);

        var response = new GetTaskByIdResponse();

        return response;
    }
}
