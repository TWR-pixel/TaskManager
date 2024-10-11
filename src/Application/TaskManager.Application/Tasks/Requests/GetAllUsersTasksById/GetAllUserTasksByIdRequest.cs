using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.Task.Specifications;

namespace TaskManager.Application.Tasks.Requests.GetAllUsersTasksById;

/// <summary>
/// Returns all user's tasks by id in database
/// </summary>
public sealed class GetAllUserTasksByIdRequest : RequestBase<GetAllUserTasksByIdResponse>
{
    public required int UserId { get; init; }
}

public sealed class GetAllUserTasksByIdResponse : ResponseBase
{
    public required int UserId { get; init; }
    public required string UserName { get; set; }
    public required string UserEmail { get; set; }

    public required IEnumerable<UserTaskByIdResponse> UserTasks { get; set; }

    public sealed class UserTaskByIdResponse
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        public required bool IsCompleted { get; set; }
        public required bool IsInProgress { get; set; }
        public required DateTime CreatedAt { get; set; }

    }
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
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        if (userQueryResult.Tasks is null)
        {
            var nullTasksResponse = new GetAllUserTasksByIdResponse
            {
                UserId = request.UserId,
                UserName = userQueryResult.Username,
                UserEmail = userQueryResult.EmailLogin,
                UserTasks = [] // null tasks
            };

            return nullTasksResponse;
        }

        var response = new GetAllUserTasksByIdResponse
        {
            UserId = request.UserId,
            UserName = userQueryResult.Username,
            UserEmail = userQueryResult.EmailLogin,

            UserTasks = userQueryResult.Tasks.Select(t => new GetAllUserTasksByIdResponse.UserTaskByIdResponse
            {
                Content = t.Content,
                IsCompleted = t.IsCompleted,
                IsInProgress = t.IsInProgress,
                Title = t.Title,
                CreatedAt = t.CreatedAt,
            }),
        };

        return response;
    }
}
