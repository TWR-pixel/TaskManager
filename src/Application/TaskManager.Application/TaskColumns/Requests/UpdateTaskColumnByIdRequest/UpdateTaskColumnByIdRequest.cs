using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.UpdateTaskColumnByIdRequest;

public sealed class UpdateTaskColumnByIdRequest : RequestBase<UpdateTaskColumnByIdResponse>
{
    public required int TaskColumnId { get; set; }
    public int? UserId { get; set; } // new user id
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateTaskColumnByIdResponse : ResponseBase
{
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateTaskColumnByIdRequestHandler : RequestHandlerBase<UpdateTaskColumnByIdRequest, UpdateTaskColumnByIdResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnsRepo;
    private readonly EfRepositoryBase<UserEntity> _usersRepo;

    public UpdateTaskColumnByIdRequestHandler(EfRepositoryBase<TaskColumnEntity> taskColumnsRepo, EfRepositoryBase<UserEntity> usersRepo)
    {
        _taskColumnsRepo = taskColumnsRepo;
        _usersRepo = usersRepo;
    }

    public override async Task<UpdateTaskColumnByIdResponse> Handle(UpdateTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var taskColumnQueryResult = await _taskColumnsRepo.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id '{request.TaskColumnId}' not found");

        if (request.Name != null) taskColumnQueryResult.Name = request.Name;
        if (request.Description != null) taskColumnQueryResult.Description = request.Description;
        if (request.UserId != null)
        {
            var userQueryResult = await _usersRepo.GetByIdAsync((int)request.UserId, cancellationToken)
                ?? throw new EntityNotFoundException($"User with id '{request.UserId}' not found");

            taskColumnQueryResult.Owner = userQueryResult;
        }


        var response = new UpdateTaskColumnByIdResponse()
        {
            Description = taskColumnQueryResult.Description,
            Name = taskColumnQueryResult.Name,
            UserId = taskColumnQueryResult.Owner.Id
        };

        await _taskColumnsRepo.UpdateAsync(taskColumnQueryResult, cancellationToken);

        return response;
    }
}