using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Tasks.Requests.Create;

public sealed record CreateTaskRequest(int UserId,
                                       int ColumnId,
                                       string Title,
                                       string Content,
                                       DateOnly? DoTo,
                                       bool IsCompleted = false,
                                       bool IsInProgress = true) : RequestBase<CreateTaskResponse>;

public sealed record CreateTaskResponse(int Id, string Title, string Content) : ResponseBase;

public sealed class CreateTaskRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<CreateTaskRequest, CreateTaskResponse>(unitOfWork)
{
    public override async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var userOwner = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("User not found by id " + request.UserId);

        var taskColumn = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.ColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("Column not found by id " + request.ColumnId);

        var taskEntity = new UserTaskEntity(request.IsCompleted,
                                            request.IsInProgress,
                                            userOwner,
                                            taskColumn,
                                            request.Title,
                                            request.Content,
                                            request.DoTo);

        var queryResult = await UnitOfWork.UserTasks.AddAsync(taskEntity, cancellationToken);

        var response = new CreateTaskResponse(queryResult.Id, queryResult.Title, queryResult.Content);

        return response;
    }
}
