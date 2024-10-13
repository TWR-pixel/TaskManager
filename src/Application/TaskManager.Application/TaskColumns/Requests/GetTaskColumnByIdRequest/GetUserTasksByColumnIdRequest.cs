using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest;

/// <summary>
/// returns new object with task column and it's user tasks.
/// </summary>
public sealed record GetUserTasksByColumnIdRequest : RequestBase<GetUserTasksByColumnIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed record GetUserTasksByColumnIdResponse : ResponseBase
{
    public required string TaskColumnName { get; set; }

    public required IEnumerable<UserTasksInColumnResponse> AllTasksInColumn { get; set; }

    public sealed record UserTasksInColumnResponse
    {
        [SetsRequiredMembers]
        public UserTasksInColumnResponse(int userTaskId,
                                         string userTaskTitle,
                                         string userTaskContent,
                                         bool isInProgress,
                                         bool isCompleted,
                                         DateTime createdAt)
        {
            UserTaskId = userTaskId;
            UserTaskTitle = userTaskTitle;
            UserTaskContent = userTaskContent;
            IsInProgress = isInProgress;
            IsCompleted = isCompleted;
            CreatedAt = createdAt;
        }

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
    public GetUserTasksByColumnIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetUserTasksByColumnIdResponse> Handle(GetUserTasksByColumnIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id {request.TaskColumnId} not found");

        queryResult.TasksInColumn ??= []; // if null initialize empty array

        var response = new GetUserTasksByColumnIdResponse
        {
            TaskColumnName = queryResult.Name,

            AllTasksInColumn = queryResult.TasksInColumn.Select(
                static t => new GetUserTasksByColumnIdResponse.UserTasksInColumnResponse(
                    t.Id,
                    t.Title,
                    t.Content,
                    t.IsInProgress,
                    t.IsCompleted,
                    t.CreatedAt))
        };

        return response;
    }
}
