using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;

public sealed class DeleteAndSaveTaskColumnByIdRequest : RequestBase<DeleteAndSaveTaskColumnByIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed class DeleteAndSaveTaskColumnByIdResponse : ResponseBase
{
}

public sealed class DeleteAndSaveTaskColumnByIdRequestHandler
    : RequestHandlerBase<DeleteAndSaveTaskColumnByIdRequest, DeleteAndSaveTaskColumnByIdResponse>
{
    private readonly EfRepository<TaskEntity> _tasksRepo;

    public DeleteAndSaveTaskColumnByIdRequestHandler(EfRepository<TaskEntity> tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    public override async Task<DeleteAndSaveTaskColumnByIdResponse> Handle(DeleteAndSaveTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await _tasksRepo.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("task column not found by id " + request.TaskColumnId);


        await _tasksRepo.DeleteAsync(entity, cancellationToken);

        var response = new DeleteAndSaveTaskColumnByIdResponse();

        return response;
    }
}
