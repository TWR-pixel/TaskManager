using TaskManager.Application.Common.Requests;
using TaskManager.Application.UserTaskColumn.Requests.Queries.GetAllUserTasksColumnsByIdRequest.Dtos;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTaskColumn.Requests.Queries.GetAllUserTasksColumnsByIdRequest;

public sealed record GetAllUserTaskColumnsByIdWithTasksRequest : RequestBase<GetAllUserTaskColumnsByIdWithTasksResponse>
{
    public required int UserId { get; set; }
}

public sealed class GetAllUserTaskColumnsByIdWithTasksRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<GetAllUserTaskColumnsByIdWithTasksRequest, GetAllUserTaskColumnsByIdWithTasksResponse>(unitOfWork)
{
    public async override Task<GetAllUserTaskColumnsByIdWithTasksResponse> Handle(GetAllUserTaskColumnsByIdWithTasksRequest request,
                                                                         CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.GetAllUserTaskColumnsWithTasksByIdAsync(request.UserId, cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        queryResult.UserTaskColumns ??= [];

        var response = new GetAllUserTaskColumnsByIdWithTasksResponse
        {
            Username = queryResult.UserName,

            TaskColumns = queryResult.UserTaskColumns.Select(u =>
                new UserTaskColumnsResponse(u.Id,
                                             u.Title,
                                             u.Description ?? "",
                                             u.TasksInColumn?.Select(t => new UserTaskResponse(t.Title,
                                                                                               t.Description,
                                                                                               t.IsInProgress,
                                                                                               t.IsCompleted,
                                                                                               t.CreatedAt,
                                                                                               t.CompletedAt,
                                                                                               t.Id,
                                                                                               t.TaskColumn.Id))
                                             )
                )
        };

        return response;
    }
}
