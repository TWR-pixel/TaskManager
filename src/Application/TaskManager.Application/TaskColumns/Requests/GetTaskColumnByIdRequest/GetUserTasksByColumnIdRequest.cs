using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest;

/// <summary>
/// returns new object with task column and it's user tasks.
/// </summary>
public sealed class GetUserTasksByColumnIdRequest : RequestBase<GetUserTasksByColumnIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed class GetUserTasksByColumnIdResponse : ResponseBase
{
    public required string TaskColumnName { get; set; }

    public required IEnumerable<UserTasksInColumnResponse> AllTasksInColumn { get; set; }

    public sealed class UserTasksInColumnResponse
    {
        public required int UserTaskId { get; set; }
        public required string UserTaskTitle { get; set; }
        public required string UserTaskContent { get; set; }
        public required bool IsInProgress { get; set; }
        public required bool IsCompleted { get; set; }

        public required DateTime CreatedAt { get; set; }
    }
}

public sealed class GetUserTasksByColumnIdRequestHandler : RequestHandlerBase<GetUserTasksByColumnIdRequest, GetUserTasksByColumnIdResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _columnsRepo;

    public GetUserTasksByColumnIdRequestHandler(EfRepositoryBase<TaskColumnEntity> columnsRepo)
    {
        _columnsRepo = columnsRepo;
    }

    public override async Task<GetUserTasksByColumnIdResponse> Handle(GetUserTasksByColumnIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _columnsRepo.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id {request.TaskColumnId} not found");

        if (queryResult.TasksInColumn is null)
        {
            var nullTasksInColumnResponse = new GetUserTasksByColumnIdResponse
            {
                TaskColumnName = queryResult.Name,
                AllTasksInColumn = [],
            };

            return nullTasksInColumnResponse;
        }

        var response = new GetUserTasksByColumnIdResponse
        {
            TaskColumnName = queryResult.Name,

            AllTasksInColumn = queryResult.TasksInColumn.Select(t => new GetUserTasksByColumnIdResponse.UserTasksInColumnResponse
            {
                CreatedAt = t.CreatedAt,
                IsCompleted = t.IsCompleted,
                IsInProgress = t.IsInProgress,
                UserTaskContent = t.Content,
                UserTaskId = t.Id,
                UserTaskTitle = t.Title,
            })
        };

        return response;
    }
}
