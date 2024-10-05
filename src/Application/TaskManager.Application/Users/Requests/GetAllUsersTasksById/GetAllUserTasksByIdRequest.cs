using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.GetAllUsersTasksById;

/// <summary>
/// Returns all user's tasks by id in database
/// </summary>
public sealed class GetAllUserTasksByIdRequest : RequestBase<GetAllUserTasksByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed class GetAllUserTasksByIdResponse : ResponseBase
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required IEnumerable<GetUserTasksByIdResponse> UserTasks { get; set; }
}

public sealed class GetAllUserTasksByIdRequestHandler : RequestHandlerBase<GetAllUserTasksByIdRequest, GetAllUserTasksByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo;

    public GetAllUserTasksByIdRequestHandler(EfRepositoryBase<UserEntity> usersRepo)
    {
        _usersRepo = usersRepo;
    }

    public override async Task<GetAllUserTasksByIdResponse> Handle(GetAllUserTasksByIdRequest request, CancellationToken cancellationToken)
    {
        var userQueryResult = await _usersRepo.SingleOrDefaultAsync(new GetAllUserTasksByIdSpecification(request.UserId), cancellationToken)
            ?? throw new EntityNotFoundException($"User by id {request.UserId} not found");

        if (userQueryResult.Tasks is null)
        {
            var nullTasksResponse = new GetAllUserTasksByIdResponse
            {
                UserId = request.UserId,
                UserName = userQueryResult.Username,
                UserTasks = [] // empty tasks
            };

            return nullTasksResponse;
        }

        var response = new GetAllUserTasksByIdResponse
        {
            UserTasks = userQueryResult.Tasks.Select(t => new GetUserTasksByIdResponse
            {
                Content = t.Content,
                IsCompleted = t.IsCompleted,
                IsInProgress = t.IsInProgress,
                Title = t.Title,
            }),

            UserId = request.UserId,
            UserName = userQueryResult.Username,
        };

        return response;
    }
}
