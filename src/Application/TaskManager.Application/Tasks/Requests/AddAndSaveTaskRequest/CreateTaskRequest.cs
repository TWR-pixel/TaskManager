using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Application.Tasks.Requests.AddAndSaveTaskRequest;

public sealed record CreateTaskRequest : RequestBase<CreateTaskResponse>
{
    public required int UserId { get; set; }
    public required int ColumnId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsInProgress { get; set; } = true;
    public DateOnly? DoTo { get; set; }

}

public sealed record CreateTaskResponse : ResponseBase
{
    public required int CreatedTaskId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}

public sealed class CreateTaskRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<CreateTaskRequest, CreateTaskResponse>(unitOfWork)
{
    public override async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var userOwner = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("User not found by id " + request.UserId);

        var taskColumn = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.ColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("Column not found by id " + request.ColumnId);

        var taskEntity = new UserTaskEntity
        {
            Title = request.Title,
            Content = request.Content,
            IsCompleted = request.IsCompleted,
            IsInProgress = request.IsInProgress,
            TaskColumn = taskColumn,
            Owner = userOwner,
            DoTo = request.DoTo
        };

        var queryResult = await UnitOfWork.UserTasks.AddAsync(taskEntity, cancellationToken);

        var response = new CreateTaskResponse
        {
            CreatedTaskId = queryResult.Id,
            Content = queryResult.Content,
            Title = queryResult.Title,
        };

        return response;
    }
}
