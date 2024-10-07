using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;

public sealed class DeleteTaskColumnByIdRequest : RequestBase<DeleteTaskColumnByIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed class DeleteTaskColumnByIdResponse : ResponseBase
{
}

public sealed class DeleteTaskColumnByIdRequestHandler
: RequestHandlerBase<DeleteTaskColumnByIdRequest, DeleteTaskColumnByIdResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _tasksRepo;

    public DeleteTaskColumnByIdRequestHandler(EfRepositoryBase<TaskColumnEntity> tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    public override async Task<DeleteTaskColumnByIdResponse> Handle(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await _tasksRepo.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("task column not found by id " + request.TaskColumnId);

        await _tasksRepo.DeleteAsync(entity, cancellationToken);

        var response = new DeleteTaskColumnByIdResponse();

        return response;
    }
}
