using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Common.Exceptions;

namespace TaskManager.Application.Tasks.Requests.GetById;

public sealed record GetTaskByIdRequest(int TaskId) : RequestBase<GetTaskByIdResponse>;
public sealed record GetTaskByIdResponse(string Title,
                                         string Description,
                                         bool IsCompleted,
                                         bool IsInProgress,
                                         DateTime CreatedAt,
                                         DateOnly? CompletedAt = null) : ResponseBase;

public sealed class GetTaskByIdRequetHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetTaskByIdRequest, GetTaskByIdResponse>(unitOfWork)
{
    public override async Task<GetTaskByIdResponse> Handle(GetTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task not found by id '{request.TaskId}");

        var response = new GetTaskByIdResponse(queryResult.Title,
                                               queryResult.Description,
                                               queryResult.IsCompleted,
                                               queryResult.IsInProgress,
                                               queryResult.CreatedAt,
                                               queryResult.CompletedAt);

        return response;
    }
}
