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
    public required string Title { get; set; }
    public required string Content { get; set; }

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

    public required DateTime CreatedAt { get; set; }
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
        var result = await _tasksRepo.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new EntityNotFoundException("Task not found by id = " + request.TaskId);

        var response = new GetTaskByIdResponse
        {
            Title = result.Title,
            Content = result.Content,
            CreatedAt = result.CreatedAt,
            IsCompleted = result.IsCompleted,
            IsInProgress = result.IsInProgress,
        };

        return response;
    }
}
