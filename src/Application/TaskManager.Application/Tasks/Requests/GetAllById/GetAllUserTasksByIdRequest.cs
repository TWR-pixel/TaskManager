using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Tasks.Specifications;

namespace TaskManager.Application.Tasks.Requests.GetAllById;

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
        var userQueryResult = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadAllUserTasksByIdSpecification(request.UserId), cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        userQueryResult.Tasks ??= [];

        var response = new GetAllUserTasksByIdResponse
        (
            request.UserId,
            userQueryResult.Username,
            userQueryResult.EmailLogin,

            userQueryResult.Tasks.Select(static t => new UserTaskByIdResponse(t.Title,
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
