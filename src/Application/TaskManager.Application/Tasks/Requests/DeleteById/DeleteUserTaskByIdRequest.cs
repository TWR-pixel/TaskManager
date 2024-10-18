using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Tasks.Requests.DeleteById;

public sealed record DeleteUserTaskByIdRequest(int TaskId) : RequestBase<DeleteUserTaskByIdResponse>;
public sealed record DeleteUserTaskByIdResponse : ResponseBase;

public sealed class DeleteUserTaskByIdRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<DeleteUserTaskByIdRequest, DeleteUserTaskByIdResponse>(unitOfWork)
{
    public override async Task<DeleteUserTaskByIdResponse> Handle(DeleteUserTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task with id {request.TaskId} not found");

        await UnitOfWork.UserTasks.DeleteAsync(queryResult, cancellationToken);

        var response = new DeleteUserTaskByIdResponse();

        return response;
    }
}
