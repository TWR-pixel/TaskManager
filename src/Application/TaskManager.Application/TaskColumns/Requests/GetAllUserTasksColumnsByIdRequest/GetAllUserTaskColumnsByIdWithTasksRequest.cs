using TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.TaskColumns.Specifications;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest;

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
                ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

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
