using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTaskColumn.Requests.Queries.GetById;

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
        public TasksInColumnResponse(int Id,
                                         string title,
                                         string? description,
                                         bool isInProgress,
                                         bool isCompleted,
                                         DateTime createdAt,
                                         DateOnly? completedAt)
        {
            this.Id = Id;
            Title = title;
            Description = description;
            IsInProgress = isInProgress;
            IsCompleted = isCompleted;
            CreatedAt = createdAt;
            CompletedAt = completedAt;
        }

        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string? Description { get; set; }
        public required bool IsInProgress { get; set; }
        public required bool IsCompleted { get; set; }

        public required DateTime CreatedAt { get; set; }
        public DateOnly? CompletedAt { get; set; }
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
            ?? throw new TaskColumnNotFoundException(request.TaskColumnId);

        queryResult.TasksInColumn ??= []; // if null initialize empty array

        var response = new GetTasksByColumnIdResponse
        {
            TaskColumnName = queryResult.Title,

            AllTasksInColumn = queryResult.TasksInColumn.Select(
                static t => new GetTasksByColumnIdResponse.TasksInColumnResponse(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsInProgress,
                    t.IsCompleted,
                    t.CreatedAt,
                    t.CompletedAt))
        };

        return response;
    }
}
