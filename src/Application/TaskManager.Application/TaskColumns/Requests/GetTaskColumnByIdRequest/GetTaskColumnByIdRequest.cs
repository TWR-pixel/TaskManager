using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest;

/// <summary>
/// Returns all tasks in column
/// </summary>
public sealed class GetTaskColumnByIdRequest : RequestBase<GetTaskColumnByIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed class GetTaskColumnByIdResponse : ResponseBase
{
}

public sealed class GetTaskColumnByIdRequestHandler : RequestHandlerBase<GetTaskColumnByIdRequest, GetTaskColumnByIdResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnsRepo;

    public GetTaskColumnByIdRequestHandler(EfRepositoryBase<TaskColumnEntity> taskColumnsRepo)
    {
        _taskColumnsRepo = taskColumnsRepo;
    }

    public override async Task<GetTaskColumnByIdResponse> Handle(GetTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _taskColumnsRepo.GetByIdAsync(request.TaskColumnId, cancellationToken);

        var response = new GetTaskColumnByIdResponse();

        return new GetTaskColumnByIdResponse();
    }
}
