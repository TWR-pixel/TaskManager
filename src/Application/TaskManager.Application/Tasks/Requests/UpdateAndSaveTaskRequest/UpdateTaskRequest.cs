using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;

namespace TaskManager.Application.Tasks.Requests.UpdateAndSaveTaskRequest;

public sealed record UpdateTaskRequest(int UpdatingTaskId,
                                       int? ColumnId = null,
                                       string? Title = null,
                                       string? Description = null,
                                       bool? IsCompleted = null,
                                       bool? IsInProgress = null,
                                       DateOnly? ComplitedAt = null) : RequestBase<UpdateTaskResponse>;

public sealed record UpdateTaskResponse : ResponseBase
{
    [SetsRequiredMembers]
    public UpdateTaskResponse(string? title, string? description, bool? isCompleted, bool? isInProgress, DateOnly? complitedAt, int id)
    {
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        IsInProgress = isInProgress;
        ComplitedAt = complitedAt;
        Id = id;
    }

    public required int Id { get; set; }
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public bool? IsCompleted { get; set; } = null;
    public bool? IsInProgress { get; set; } = null;
    public DateOnly? ComplitedAt { get; set; }
}

public sealed class UpdateTaskRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<UpdateTaskRequest, UpdateTaskResponse>(unitOfWork)
{
    public override async Task<UpdateTaskResponse> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var entityForUpdate = await UnitOfWork.UserTasks.GetByIdAsync(request.UpdatingTaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task by id {request.UpdatingTaskId} not found. ");

        if (request.Title != null)
            entityForUpdate.Title = request.Title;

        if (request.ColumnId != null)
        {
            var columnEntity = await UnitOfWork.UserTaskColumns.GetByIdAsync((int)request.ColumnId, cancellationToken)
                ?? throw new EntityNotFoundException($"Task column by id {request.ColumnId} not found. ");

            entityForUpdate.TaskColumn = columnEntity;
        }

        if (request.Description != null)
            entityForUpdate.Description = request.Description;

        if (request.IsCompleted != null)
            entityForUpdate.IsCompleted = (bool)request.IsCompleted;

        if (request.IsInProgress != null)
            entityForUpdate.IsInProgress = (bool)request.IsInProgress;

        if (request.ComplitedAt != null)
            entityForUpdate.ComplitedAt = request.ComplitedAt;

        await UnitOfWork.UserTasks.UpdateAsync(entityForUpdate, cancellationToken);

        var response = new UpdateTaskResponse(request.Title,
                                              request.Description,
                                              request.IsInProgress,
                                              request.IsInProgress,
                                              request.ComplitedAt,
                                              request.UpdatingTaskId);

        return response;
    }
}