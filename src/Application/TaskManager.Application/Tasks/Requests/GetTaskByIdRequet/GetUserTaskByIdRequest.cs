using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Application.Tasks.Requests.GetTaskByIdRequet;

public sealed record GetUserTaskByIdRequest(int TaskId) : RequestBase<GetUserTaskByIdResponse>;
public sealed record GetUserTaskByIdResponse(string Title, string Content, bool IsCompleted, bool IsInProgress, DateTime CreatedAt, DateOnly? DoTo = null) : ResponseBase;

public sealed class GetUserTaskByIdRequetHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetUserTaskByIdRequest, GetUserTaskByIdResponse>(unitOfWork)
{
    public override async Task<GetUserTaskByIdResponse> Handle(GetUserTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task not found by id '{request.TaskId}");

        var response = new GetUserTaskByIdResponse(queryResult.Title,
                                               queryResult.Content,
                                               queryResult.IsCompleted,
                                               queryResult.IsInProgress,
                                               queryResult.CreatedAt,
                                               queryResult.DoTo);

        return response;
    }
}
