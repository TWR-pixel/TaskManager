using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.GetById;

public sealed record GetTaskByIdRequest(int TaskId) : RequestBase<GetTaskByIdResponse>;
public sealed record GetTaskByIdResponse(string Title,
                                         string? Description,
                                         bool IsCompleted,
                                         bool IsInProgress,
                                         DateTime CreatedAt,
                                         DateOnly? CompletedAt = null) : ResponseBase;

public sealed class GetTaskByIdRequetHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetTaskByIdRequest, GetTaskByIdResponse>(unitOfWork)
{
    public override async Task<GetTaskByIdResponse> Handle(GetTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new TaskNotFoundException(request.TaskId);

        var response = queryResult.ToGetTaskByIdResponse();

        return response;
    }
}
