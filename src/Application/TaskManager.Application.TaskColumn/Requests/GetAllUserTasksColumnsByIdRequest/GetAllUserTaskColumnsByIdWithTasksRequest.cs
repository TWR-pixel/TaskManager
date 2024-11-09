using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.TaskColumn.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.TaskColumns.Specifications;

namespace TaskManager.Application.TaskColumn.Requests.GetAllUserTasksColumnsByIdRequest;

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
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadAllUserTaskColumnsWithTasksByIdSpec(request.UserId), cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        queryResult.TaskColumns ??= [];

        var response = new GetAllUserTaskColumnsByIdWithTasksResponse
        {
            Username = queryResult.Username,

            TaskColumns = queryResult.TaskColumns.Select(u => // select not right, maybe create domain model with this
                new UserTaskColumnsResponse(u.Id,
                                             u.Title,
                                             u.Description ?? "", // if null return ''
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
