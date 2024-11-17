using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumn.Requests.GetById;

/// <summary>
/// Returns all tasks in column
/// </summary>
/// <param name="TaskColumnId"></param>
public sealed record GetTaskColumnByIdRequest(int TaskColumnId) : RequestBase<UserTaskColumnDto>;

public sealed class GetTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<GetTaskColumnByIdRequest, UserTaskColumnDto>(unitOfWork)
{
    public override async Task<UserTaskColumnDto> Handle(GetTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.UserTaskColumns
            .GetByIdWithOwnerAndUserTasksAsync(request.TaskColumnId, cancellationToken)
                ?? throw new TaskColumnNotFoundException(request.TaskColumnId);

        var response = queryResult.ToResponse();

        return response;
    }
}
