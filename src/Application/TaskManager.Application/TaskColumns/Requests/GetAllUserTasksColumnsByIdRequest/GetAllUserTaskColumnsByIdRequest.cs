using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.TaskColumn.Specifications;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest;

public sealed class GetAllUserTaskColumnsByIdRequest : RequestBase<GetAllUserTaskColumnsByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed class GetAllUserTaskColumnsByIdResponse : ResponseBase
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required IEnumerable<UserTasksColumnsResponse> UserTaskColumns { get; set; }

    public sealed class UserTasksColumnsResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Content { get; set; }
    }
}

public sealed class GetAllUserTaskColumnsByIdRequestHandler
    : RequestHandlerBase<GetAllUserTaskColumnsByIdRequest, GetAllUserTaskColumnsByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo;

    public GetAllUserTaskColumnsByIdRequestHandler(EfRepositoryBase<UserEntity> usersRepo)
    {
        _usersRepo = usersRepo;
    }

    public async override Task<GetAllUserTaskColumnsByIdResponse> Handle(GetAllUserTaskColumnsByIdRequest request,
                                                                                      CancellationToken cancellationToken)
    {
        var queryResult = await _usersRepo
            .SingleOrDefaultAsync(new GetAllTaskColumnsWithTasksByIdSpec(request.UserId), cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        if (queryResult.TaskColumns is null)
        {
            var nullTaskColumnsResponse = new GetAllUserTaskColumnsByIdResponse
            {
                UserId = request.UserId,
                UserName = queryResult.Username,
                UserTaskColumns = [] // null columns
            };

            return nullTaskColumnsResponse;
        }

        var response = new GetAllUserTaskColumnsByIdResponse
        {
            UserId = queryResult.Id,
            UserName = queryResult.Username,
            UserTaskColumns = queryResult.TaskColumns.Select(u => new GetAllUserTaskColumnsByIdResponse.UserTasksColumnsResponse
            {
                Content = u.Description ?? "Empty", // if null return empty
                Id = u.Id,
                Name = u.Name,
            })
        };

        return response;
    }
}
