using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumns.Requests.GetById;

/// <summary>
/// returns new object with task column and it's user tasks.
/// </summary>
public sealed record GetTasksByTaskColumnIdRequest : RequestBase<GetTasksByColumnIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed record GetTasksByColumnIdResponse : ResponseBase
{
    public required string TaskColumnName { get; set; }

    public required IEnumerable<TasksInColumnResponse> AllTasksInColumn { get; set; }

    public sealed record TasksInColumnResponse
    {
        [SetsRequiredMembers]
        public TasksInColumnResponse(int userTaskId,
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

public sealed class GetTasksByTaskColumnIdRequestHandler : RequestHandlerBase<GetTasksByTaskColumnIdRequest, GetTasksByColumnIdResponse>
{
    public GetTasksByTaskColumnIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetTasksByColumnIdResponse> Handle(GetTasksByTaskColumnIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id {request.TaskColumnId} not found");

        queryResult.TasksInColumn ??= []; // if null initialize empty array

        var response = new GetTasksByColumnIdResponse
        {
            TaskColumnName = queryResult.Name,

            AllTasksInColumn = queryResult.TasksInColumn.Select(
                static t => new GetTasksByColumnIdResponse.TasksInColumnResponse(
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
