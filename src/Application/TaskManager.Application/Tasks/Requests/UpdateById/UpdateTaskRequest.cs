using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Tasks.Requests.UpdateById;

public sealed record UpdateTaskRequest(int UpdatingTaskId,
                                       int? ColumnId = null,
                                       string? Title = null,
                                       string? Content = null,
                                       bool? IsCompleted = null,
                                       bool? IsInProgress = null) : RequestBase<UpdateTaskResponse>;

public sealed record UpdateTaskResponse(string? Title = null,
                                        string? Content = null,
                                        bool? IsCompleted = null,
                                        bool? IsInProgress = null) : ResponseBase;

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

        if (request.Content != null)
            entityForUpdate.Content = request.Content;

        if (request.IsCompleted != null)
            entityForUpdate.IsCompleted = (bool)request.IsCompleted;

        if (request.IsInProgress != null)
            entityForUpdate.IsInProgress = (bool)request.IsInProgress;


        await UnitOfWork.UserTasks.UpdateAsync(entityForUpdate, cancellationToken);

        var response = new UpdateTaskResponse(request.Title,
                                              request.Content,
                                              request.IsInProgress,
                                              request.IsInProgress);

        return response;
    }
}