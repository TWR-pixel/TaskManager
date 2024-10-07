using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest;

/// <summary>
/// Returns all tasks in column
/// </summary>
public sealed class GetTaskColumnByIdRequest : RequestBase<GetTaskColumnByIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed class GetTaskColumnByIdResponse : ResponseBase
{
    public required int TaskColumnId { get; set; }

    public required IEnumerable<UserTasksColumnResponse> Tasks { get; set; }

    public sealed class UserTasksColumnResponse
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required bool IsCompleted { get; set; }
        public required bool IsInProgress { get; set; }
    }
}

public sealed class GetTaskColumnByIdRequestHandler : RequestHandlerBase<GetTaskColumnByIdRequest, GetTaskColumnByIdResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnsRepo;

    public GetTaskColumnByIdRequestHandler(EfRepositoryBase<TaskColumnEntity> taskColumnsRepo)
    {
        _taskColumnsRepo = taskColumnsRepo;
    }

    public override async Task<GetTaskColumnByIdResponse> Handle(GetTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _taskColumnsRepo.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"User task column with id {request.TaskColumnId} not found");

        if (queryResult.TasksInColumn is null)
        {
            var nullTasksInColumnResponse = new GetTaskColumnByIdResponse
            {
                TaskColumnId = request.TaskColumnId,
                Tasks = []
            };

            return nullTasksInColumnResponse;
        }

        var response = new GetTaskColumnByIdResponse()
        {
            TaskColumnId = queryResult.Id,
            Tasks = queryResult.TasksInColumn.Select(t => new GetTaskColumnByIdResponse.UserTasksColumnResponse
            {
                Title = t.Title,
                Content = t.Content,
                IsCompleted = t.IsCompleted,
                IsInProgress = t.IsInProgress,
            })
        };

        return response;
    }
}
