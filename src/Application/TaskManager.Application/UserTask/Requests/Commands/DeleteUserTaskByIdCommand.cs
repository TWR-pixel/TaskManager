using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.Commands;

public sealed record DeleteUserTaskByIdCommand(int TaskId) : CommandBase<DeleteUserTaskByIdResponse>;
public sealed record DeleteUserTaskByIdResponse : ResponseBase;

public sealed class DeleteUserTaskByIdCommandHandler(IUnitOfWork unitOfWork)
    : CommandHandlerBase<DeleteUserTaskByIdCommand, DeleteUserTaskByIdResponse>(unitOfWork)
{
    public override async Task<DeleteUserTaskByIdResponse> Handle(DeleteUserTaskByIdCommand request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTasks.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new TaskNotFoundException(request.TaskId);

        await UnitOfWork.UserTasks.DeleteAsync(queryResult, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = new DeleteUserTaskByIdResponse();

        return response;
    }
}
