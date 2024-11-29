using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.Commands;

public sealed record CreateTaskCommand(int UserId,
                                       int ColumnId,
                                       string Title,
                                       string Description,
                                       DateOnly? CompletedAt,
                                       bool IsCompleted = false,
                                       bool IsInProgress = true) : CommandBase<CreateTaskResponse>;

public sealed record CreateTaskResponse(int CreatedTaskId,
                                        int ColumnId,
                                        string Title,
                                        string Description,
                                        bool IsCompleted,
                                        bool IsInProgress,
                                        DateOnly? CompletedAt);

public sealed class CreateTaskCommandHandler(IUnitOfWork unitOfWork) : CommandHandlerBase<CreateTaskCommand, CreateTaskResponse>(unitOfWork)
{
    public override async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
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

        var result = await UnitOfWork.UserTasks.AddAsync(taskEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = new CreateTaskResponse(result.Id, taskEntity.TaskColumn.Id, result.Title, result.Description, result.IsCompleted, result.IsInProgress, result.CompletedAt);

        return response;
    }
}
