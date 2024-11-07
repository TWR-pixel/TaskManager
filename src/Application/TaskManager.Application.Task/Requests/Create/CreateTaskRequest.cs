using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.Create;

public sealed record CreateTaskRequest(int UserId,
                                       int ColumnId,
                                       string Title,
                                       string Description,
                                       DateOnly? CompletedAt,
                                       bool IsCompleted = false,
                                       bool IsInProgress = true) : RequestBase<UserTaskDto>;

public sealed class CreateTaskRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<CreateTaskRequest, UserTaskDto>(unitOfWork)
{
    public override async Task<UserTaskDto> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var userOwner = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        var taskColumn = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.ColumnId, cancellationToken)
            ?? throw new TaskColumnNotFoundException(request.ColumnId);

        var taskEntity = new UserTaskEntity(request.IsCompleted,
                                            request.IsInProgress,
                                            userOwner,
                                            taskColumn,
                                            request.Title,
                                            request.Description,
                                            request.CompletedAt);

        await UnitOfWork.UserTasks.AddAsync(taskEntity, cancellationToken);

        var response = taskEntity.ToResponse();

        return response;
    }
}
