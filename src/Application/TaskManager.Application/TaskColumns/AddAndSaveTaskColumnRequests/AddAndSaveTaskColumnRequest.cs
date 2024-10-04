using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.AddAndSaveTaskColumnRequests;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
public sealed class AddAndSaveTaskColumnRequest : RequestBase<AddAndSaveTaskColumnResponse>
{

}

/// <summary>
/// Response for user
/// </summary>
public sealed class AddAndSaveTaskColumnResponse : ResponseBase
{
}

public sealed class AddAndSaveTaskColumnRequestHandler :
    RequestHandlerBase<AddAndSaveTaskColumnRequest, AddAndSaveTaskColumnResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnRepo;

    public AddAndSaveTaskColumnRequestHandler(EfRepositoryBase<TaskColumnEntity> taskColumnRepo)
    {
        _taskColumnRepo = taskColumnRepo;
    }

    public override async Task<AddAndSaveTaskColumnResponse> Handle
        (AddAndSaveTaskColumnRequest request, CancellationToken cancellationToken)
    {
        //var result = await _taskColumnRepo.ListAsync(, cancellationToken);

        throw new NotImplementedException();
    }
}