using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Application.Tasks.Requests.DeleteTaskByIdRequest;

public sealed record DeleteUserTaskByIdRequest : RequestBase<DeleteUserTaskByIdResponse>
{
    public required int TaskId { get; set; }
}

public sealed record DeleteUserTaskByIdResponse : ResponseBase
{
}

public sealed class DeleteUserTaskByIdRequestHandler : RequestHandlerBase<DeleteUserTaskByIdRequest, DeleteUserTaskByIdResponse>
{
    public DeleteUserTaskByIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<DeleteUserTaskByIdResponse> Handle(DeleteUserTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task with id {request.TaskId} not found");

        await UnitOfWork.UserTasks.DeleteAsync(queryResult, cancellationToken);

        var response = new DeleteUserTaskByIdResponse();

        return response;
    }
}
