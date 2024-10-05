using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Data;

namespace TaskManager.Application.Tasks.Requests.UpdatePatchAndSaveTaskRequest;

public sealed class UpdatePatchAndSaveTaskRequest : RequestBase<UpdatePatchAndSaveTaskResponse>
{
    public required int UpdatingTaskId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool? IsCompleted { get; set; }
    public bool? IsInProgress { get; set; }
}

public sealed class UpdatePatchAndSaveTaskResponse : ResponseBase
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool? IsCompleted { get; set; }
    public bool? IsInProgress { get; set; }
}

public sealed class UpdatePatchAndSaveTaskRequestHandler
    : RequestHandlerBase<UpdatePatchAndSaveTaskRequest, UpdatePatchAndSaveTaskResponse>
{
    private readonly EfRepositoryBase<TaskEntity> _taskRepo;

    public UpdatePatchAndSaveTaskRequestHandler(EfRepositoryBase<TaskEntity> taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public override async Task<UpdatePatchAndSaveTaskResponse> Handle(UpdatePatchAndSaveTaskRequest request, CancellationToken cancellationToken)
    {
        var entityForUpdate = await _taskRepo.GetByIdAsync(request.UpdatingTaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task by id {request.UpdatingTaskId} not found. ");

        if (request.Title != null)
            entityForUpdate.Title = request.Title;

        if (request.Content != null)
            entityForUpdate.Content = request.Content;

        if (request.IsCompleted != null)
            entityForUpdate.IsCompleted = (bool)request.IsCompleted;

        if (request.IsInProgress != null)
            entityForUpdate.IsInProgress = (bool)request.IsInProgress;


        await _taskRepo.UpdateAsync(entityForUpdate, cancellationToken);

        var response = new UpdatePatchAndSaveTaskResponse()
        {
            Content = request.Content,
            Title = request.Title,
            IsCompleted = request.IsCompleted,
            IsInProgress = request.IsInProgress,
        };

        return response;
    }
}