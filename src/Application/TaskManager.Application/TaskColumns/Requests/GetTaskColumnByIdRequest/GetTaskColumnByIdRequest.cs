using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.UseCases.TaskColumns.Specifications;
using static TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest.GetTaskColumnByIdResponse;

namespace TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest;

/// <summary>
/// Returns all tasks in column
/// </summary>
/// <param name="TaskColumnId"></param>
public sealed record GetTaskColumnByIdRequest(int TaskColumnId) : RequestBase<GetTaskColumnByIdResponse>;

public sealed record GetTaskColumnByIdResponse(int TaskColumnId,
                                               string Title,
                                               string? Description,
                                               IEnumerable<UserTasksColumnResponse> Tasks) : ResponseBase
{
    public sealed record UserTasksColumnResponse
    {
        [SetsRequiredMembers]
        public UserTasksColumnResponse(bool isCompleted, bool isInProgress, string title, string content, DateOnly? complitedAt)
        {
            IsCompleted = isCompleted;
            IsInProgress = isInProgress;
            Title = title;
            Description = content;
            ComplitedAt = complitedAt;
        }

        public required string Title { get; set; }
        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required bool IsCompleted { get; set; }
        public required bool IsInProgress { get; set; }
        public required DateOnly? ComplitedAt { get; set; }
    }
}

public sealed class GetTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<GetTaskColumnByIdRequest, GetTaskColumnByIdResponse>(unitOfWork)
{
    public override async Task<GetTaskColumnByIdResponse> Handle(GetTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTaskColumns
            .SingleOrDefaultAsync(new GetTaskColumnsByIdWithTasksSpecification(request.TaskColumnId), cancellationToken)
            ?? throw new EntityNotFoundException($"User task column with id {request.TaskColumnId} not found");

        queryResult.TasksInColumn ??= [];

        var response = new GetTaskColumnByIdResponse
        (
            queryResult.Id,
            queryResult.Title,
            queryResult.Description,
            queryResult.TasksInColumn.Select(static t =>
            {
                return new UserTasksColumnResponse(t.IsCompleted, t.IsInProgress, t.Title, t.Description, t.ComplitedAt);
            })
        );

        return response;
    }
}
