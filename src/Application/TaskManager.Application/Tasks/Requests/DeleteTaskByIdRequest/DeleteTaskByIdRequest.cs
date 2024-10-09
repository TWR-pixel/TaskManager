using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Data;

namespace TaskManager.Application.Tasks.Requests.DeleteTaskByIdRequest;

public sealed class DeleteTaskByIdRequest : RequestBase<DeleteTaskByIdResponse>
{
    public required int TaskId { get; set; }
}

public sealed class DeleteTaskByIdResponse : ResponseBase
{
}

public sealed class DeleteTaskByIdRequestHandler : RequestHandlerBase<DeleteTaskByIdRequest, DeleteTaskByIdResponse>
{
    private readonly EfRepositoryBase<UserTaskEntity> _tasksRepo;

    public DeleteTaskByIdRequestHandler(EfRepositoryBase<UserTaskEntity> tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    public override async Task<DeleteTaskByIdResponse> Handle(DeleteTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _tasksRepo.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task with id {request.TaskId} not found");

        await _tasksRepo.DeleteAsync(queryResult, cancellationToken);

        var response = new DeleteTaskByIdResponse();

        return response;
    }
}
