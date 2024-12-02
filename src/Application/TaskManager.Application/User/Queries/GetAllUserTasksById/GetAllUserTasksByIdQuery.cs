using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.GetAllUserTasksById;

public sealed record GetAllUserTasksByIdQuery : QueryBase<GetAllUserTasksByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed record GetAllUserTasksByIdResponse(int UserId,
                                                 string Username,
                                                 string UserEmail,
                                                 IEnumerable<UserTaskByIdResponse> UserTasks) : ResponseBase;

public sealed class GetAllUserTasksByIdQueryHandler(IReadonlyUnitOfWork unitOfWork) : QueryHandlerBase<GetAllUserTasksByIdQuery, GetAllUserTasksByIdResponse>(unitOfWork)
{
    public override async Task<GetAllUserTasksByIdResponse> Handle(GetAllUserTasksByIdQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetAllUserTasks(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        userEntity.UserTasks ??= [];

        var response = new GetAllUserTasksByIdResponse
        (
            userEntity.Id,
            userEntity.UserName,
            userEntity.EmailLogin,

            userEntity.UserTasks.Select(static t => new UserTaskByIdResponse(t.Title,
                                                                              t.Description,
                                                                              t.IsCompleted,
                                                                              t.IsInProgress,
                                                                              t.CreatedAt,
                                                                              t.CompletedAt,
                                                                              t.Id,
                                                                              t.TaskColumn.Id))
        );

        return response;
    }
}