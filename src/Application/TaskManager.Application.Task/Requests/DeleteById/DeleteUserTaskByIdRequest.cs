using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.DeleteById;

public sealed record DeleteUserTaskByIdRequest(int TaskId) : RequestBase<DeleteUserTaskByIdResponse>;
public sealed record DeleteUserTaskByIdResponse : ResponseBase;

public sealed class DeleteUserTaskByIdRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<DeleteUserTaskByIdRequest, DeleteUserTaskByIdResponse>(unitOfWork)
{
    public override async Task<DeleteUserTaskByIdResponse> Handle(DeleteUserTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new TaskNotFoundException(request.TaskId);

        await UnitOfWork.UserTasks.DeleteAsync(queryResult, cancellationToken);

        var response = new DeleteUserTaskByIdResponse();

        return response;
    }
}
