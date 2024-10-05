using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Users.Requests.GetUsersTasksById;

public sealed class GetAllUserTasksByIdRequest : RequestBase<GetAllUserTasksByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed class GetAllUserTasksByIdResponse : ResponseBase
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required IEnumerable<TaskEntity> UserTasks { get; set; }
}

public sealed class GetAllUserTasksByIdRequestHandler
    : RequestHandlerBase<GetAllUserTasksByIdRequest, GetAllUserTasksByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _tasksRepo;

    public GetAllUserTasksByIdRequestHandler(EfRepositoryBase<UserEntity> tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    public override async Task<GetAllUserTasksByIdResponse> Handle(GetAllUserTasksByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _tasksRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User by id {request.UserId} not found");

        if (queryResult.Tasks is null)
        {
            var nullTasksResponse = new GetAllUserTasksByIdResponse
            {
                UserId = request.UserId,
                UserName = queryResult.Username,
                UserTasks = []
            };

            return nullTasksResponse;
        }

        var response = new GetAllUserTasksByIdResponse
        {
            UserTasks = queryResult.Tasks,
            UserId = request.UserId,
            UserName = queryResult.Username,
        };

        return response;
    }
}
