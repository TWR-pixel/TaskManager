using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Application.Tasks.Requests.UpdateById;

public sealed record UpdateTaskRequest(int UpdatingTaskId,
                                       int? ColumnId = null,
                                       string? Title = null,
                                       DateOnly? CompletedAt = null,
                                       string? Description = null,
                                       bool? IsCompleted = null,
                                       bool? IsInProgress = null) : RequestBase<UpdateTaskResponse>;

public sealed record UpdateTaskResponse(string? Title = null,
                                        DateOnly? CompletedAt = null,
                                        string? Description = null,
                                        bool? IsCompleted = null,
                                        bool? IsInProgress = null) : ResponseBase;

public sealed class UpdateTaskRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<UpdateTaskRequest, UpdateTaskResponse>(unitOfWork)
{
    public override async Task<UpdateTaskResponse> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var entityForUpdate = await UnitOfWork.UserTasks.GetByIdAsync(request.UpdatingTaskId, cancellationToken)
            ?? throw new EntityNotFoundException("Task", request.UpdatingTaskId);

        if (request.Title != null)
            entityForUpdate.Title = request.Title;

        if (request.ColumnId != null)
        {
            var columnEntity = await UnitOfWork.UserTaskColumns.GetByIdAsync((int)request.ColumnId, cancellationToken)
                ?? throw new TaskColumnNotFoundException((int)request.ColumnId);

            entityForUpdate.TaskColumn = columnEntity;
        }

        if (request.Description is not null)
            entityForUpdate.Description = request.Description;

        if (request.IsCompleted is not null)
            entityForUpdate.IsCompleted = (bool)request.IsCompleted;

        if (request.IsInProgress is not null)
            entityForUpdate.IsInProgress = (bool)request.IsInProgress;

        if (request.CompletedAt is not null)
            entityForUpdate.CompletedAt = request.CompletedAt;

        await UnitOfWork.UserTasks.UpdateAsync(entityForUpdate, cancellationToken);

        var response = new UpdateTaskResponse(entityForUpdate.Title,
                                              entityForUpdate.CompletedAt,
                                              entityForUpdate.Description,
                                              entityForUpdate.IsCompleted,
                                              entityForUpdate.IsInProgress);

        return response;
    }
}