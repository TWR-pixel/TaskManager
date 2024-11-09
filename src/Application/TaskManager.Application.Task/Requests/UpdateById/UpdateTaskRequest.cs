using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.UpdateById;

public sealed record UpdateTaskRequest(int UpdatingTaskId,
                                       int? ColumnId = null,
                                       string? Title = null,
                                       DateOnly? CompletedAt = null,
                                       string? Description = null,
                                       bool? IsCompleted = null,
                                       bool? IsInProgress = null) : RequestBase<UpdateTaskResponse>;

public sealed record UpdateTaskResponse(int UpdatingTaskId,
                                        int? ColumnId = null,
                                        string? Title = null,
                                        DateOnly? CompletedAt = null,
                                        string? Description = null,
                                        bool? IsCompleted = null,
                                        bool? IsInProgress = null) : ResponseBase;

public sealed class UpdateTaskRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<UpdateTaskRequest, UpdateTaskResponse>(unitOfWork)
{
    public override async Task<UpdateTaskResponse> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
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

        var response = new UpdateTaskResponse(taskEntity.Id,
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