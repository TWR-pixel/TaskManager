using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.Queries;

public sealed record GetTaskByIdQuery(int TaskId) : QueryBase<GetTaskByIdResponse>;
public sealed record GetTaskByIdResponse(string Title,
                                         string? Description,
                                         bool IsCompleted,
                                         bool IsInProgress,
                                         DateTime CreatedAt,
                                         DateOnly? CompletedAt = null) : ResponseBase;

public sealed class GetTaskByIdQueryHandler(IReadonlyUnitOfWork unitOfWork) : QueryHandlerBase<GetTaskByIdQuery, GetTaskByIdResponse>(unitOfWork)
{
    public override async Task<GetTaskByIdResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new TaskNotFoundException(request.TaskId);

        var response = queryResult.ToGetTaskByIdResponse();

        return response;
    }
}
