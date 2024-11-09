using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Tasks.Specifications;

namespace TaskManager.Application.UserTask.Requests.GetAllById;

/// <summary>
/// Returns all user's tasks by id in database
/// </summary>
public sealed record GetAllUserTasksByIdRequest(int UserId) : RequestBase<GetAllUserTasksByIdResponse>;

public sealed record GetAllUserTasksByIdResponse(int UserId,
                                                 string Username,
                                                 string UserEmail,
                                                 IEnumerable<UserTaskByIdResponse> UserTasks) : ResponseBase;

public sealed class GetAllUserTasksByIdRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<GetAllUserTasksByIdRequest, GetAllUserTasksByIdResponse>(unitOfWork)
{
    public override async Task<GetAllUserTasksByIdResponse> Handle(GetAllUserTasksByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadAllUserTasksByIdSpecification(request.UserId), cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        userEntity.Tasks ??= [];

        var response = new GetAllUserTasksByIdResponse
        (
            request.UserId,
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
