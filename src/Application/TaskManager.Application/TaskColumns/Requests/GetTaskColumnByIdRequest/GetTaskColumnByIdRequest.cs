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
    public required string Name { get; set; }
    public string? Description { get; set; }

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

public sealed class GetTaskColumnByIdRequestHandler(EfRepositoryBase<TaskColumnEntity> taskColumnsRepo) : RequestHandlerBase<GetTaskColumnByIdRequest, GetTaskColumnByIdResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnsRepo = taskColumnsRepo;

    public override async Task<GetTaskColumnByIdResponse> Handle(GetTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _taskColumnsRepo.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"User task column with id {request.TaskColumnId} not found");

        if (queryResult.TasksInColumn is null)
        {
            var nullTasksInColumnResponse = new GetTaskColumnByIdResponse
            {
                TaskColumnId = request.TaskColumnId,
                Name = queryResult.Name,
                Description = queryResult.Description,
                Tasks = []
            };

            return nullTasksInColumnResponse;
        }

        var response = new GetTaskColumnByIdResponse()
        {
            TaskColumnId = queryResult.Id,
            Name = queryResult.Name,
            Description = queryResult.Description,
            Tasks = queryResult.TasksInColumn.Select(static t => new GetTaskColumnByIdResponse.UserTasksColumnResponse // i dont like it
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
