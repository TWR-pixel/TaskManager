using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Application.Tasks.Requests.Create;

public sealed record CreateTaskRequest(int UserId,
                                       int ColumnId,
                                       string Title,
                                       string Description,
                                       DateOnly? CompletedAt,
                                       bool IsCompleted = false,
                                       bool IsInProgress = true) : RequestBase<CreateTaskResponse>;

public sealed record CreateTaskResponse(int Id,
                                        int UserId,
                                        int ColumnId,
                                        string Title,
                                        string Description,
                                        DateOnly? CompletedAt,
                                        bool IsCompleted,
                                        bool IsInProgress) : ResponseBase;

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
                                            request.Description,
                                            request.CompletedAt);

        var queryResult = await UnitOfWork.UserTasks.AddAsync(taskEntity, cancellationToken);

        var response = new CreateTaskResponse(taskEntity.Id,
                                              request.UserId,
                                              request.ColumnId,
                                              request.Title,
                                              request.Description,
                                              request.CompletedAt,
                                              request.IsCompleted,
                                              request.IsInProgress);

        return response;
    }
}
