using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.Commands;

public sealed record UpdateTaskCommand(int UpdatingTaskId,
                                       int? ColumnId = null,
                                       string? Title = null,
                                       DateOnly? CompletedAt = null,
                                       string? Description = null,
                                       bool? IsCompleted = null,
                                       bool? IsInProgress = null) : CommandBase<UpdateUserTaskResponse>;

public sealed record UpdateUserTaskResponse(int UpdatingTaskId,
                                        int? ColumnId = null,
                                        string? Title = null,
                                        DateOnly? CompletedAt = null,
                                        string? Description = null,
                                        bool? IsCompleted = null,
                                        bool? IsInProgress = null) : ResponseBase;

public sealed class UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
        : CommandHandlerBase<UpdateTaskCommand, UpdateUserTaskResponse>(unitOfWork)
{
    public override async Task<UpdateUserTaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var taskEntity = await UnitOfWork.UserTasks.GetByIdAsync(request.UpdatingTaskId, cancellationToken)
            ?? throw new TaskNotFoundException(request.UpdatingTaskId);

        if (!string.IsNullOrWhiteSpace(request.Title))
            taskEntity.Title = request.Title;

        if (request.ColumnId != null)
        {
            var columnEntity = await UnitOfWork.UserTaskColumns.GetByIdAsync((int)request.ColumnId, cancellationToken)
                ?? throw new TaskColumnNotFoundException((int)request.ColumnId);

            taskEntity.TaskColumn = columnEntity;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
            taskEntity.Description = request.Description;

        if (request.IsCompleted is not null)
            taskEntity.IsCompleted = (bool)request.IsCompleted;

        if (request.IsInProgress is not null)
            taskEntity.IsInProgress = (bool)request.IsInProgress;

        if (request.CompletedAt is not null)
            taskEntity.CompletedAt = request.CompletedAt;

        await UnitOfWork.UserTasks.UpdateAsync(taskEntity, cancellationToken);

        var response = new UpdateUserTaskResponse(taskEntity.Id,
                                              request.ColumnId,
                                              taskEntity.Title,
                                              taskEntity.CompletedAt,
                                              taskEntity.Description,
                                              taskEntity.IsCompleted,
                                              taskEntity.IsInProgress);

        await SaveChangesAsync(cancellationToken);

        return response;
    }
}