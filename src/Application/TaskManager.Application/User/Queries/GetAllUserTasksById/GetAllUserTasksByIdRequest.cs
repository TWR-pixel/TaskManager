using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.GetAllUserTasksById;

public sealed record GetAllUserTasksByIdRequest : RequestBase<GetAllUserTasksByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed record GetAllUserTasksByIdResponse(int UserId,
                                                 string Username,
                                                 string UserEmail,
                                                 IEnumerable<UserTaskByIdResponse> UserTasks) : ResponseBase;

public sealed class GetAllUserTasksByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetAllUserTasksByIdRequest, GetAllUserTasksByIdResponse>(unitOfWork)
{
    public override async Task<GetAllUserTasksByIdResponse> Handle(GetAllUserTasksByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetAllUserTasks(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        userEntity.Tasks ??= [];

        var response = new GetAllUserTasksByIdResponse
        (
            userEntity.Id,
            userEntity.Username,
            userEntity.EmailLogin,

            userEntity.Tasks.Select(static t => new UserTaskByIdResponse(t.Title,
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