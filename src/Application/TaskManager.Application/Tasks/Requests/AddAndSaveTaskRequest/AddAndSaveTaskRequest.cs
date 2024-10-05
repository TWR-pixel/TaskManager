using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Tasks.Requests.AddAndSaveTaskRequest;
 
public sealed class AddAndSaveTaskRequest : RequestBase<AddAndSaveTaskResponse>
{
    public required int UserId { get; set; }
    public required int ColumnId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsInProgress { get; set; } = true;
}

public sealed class AddAndSaveTaskResponse : ResponseBase
{
    public required int CreatedTaskId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}

public sealed class AddAndSaveTaskRequestHandler : RequestHandlerBase<AddAndSaveTaskRequest, AddAndSaveTaskResponse>
{
    private readonly EfRepositoryBase<TaskEntity> _taskRepo;
    private readonly EfRepositoryBase<UserEntity> _userRepo;
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnsRepo;

    public AddAndSaveTaskRequestHandler(EfRepositoryBase<TaskEntity> taskRepo,
                                        EfRepositoryBase<UserEntity> userRepo,
                                        EfRepositoryBase<TaskColumnEntity> taskColumnsRepo)
    {
        _taskRepo = taskRepo;
        _userRepo = userRepo;
        _taskColumnsRepo = taskColumnsRepo;
    }

    public override async Task<AddAndSaveTaskResponse> Handle(AddAndSaveTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("User not found by id " + request.UserId);

        var column = await _taskColumnsRepo.GetByIdAsync(request.ColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("Column not found by id " + request.ColumnId);

        var taskEntity = new TaskEntity
        {
            Title = request.Title,
            Content = request.Content,
            IsCompleted = request.IsCompleted,
            IsInProgress = request.IsInProgress,
            TaskColumn = column,
            Owner = user
        };

        var queryResult = await _taskRepo.AddAsync(taskEntity, cancellationToken);

        var response = new AddAndSaveTaskResponse
        {
            CreatedTaskId = queryResult.Id,
            Content = queryResult.Content,
            Title = queryResult.Title,
        };

        return response;
    }
}
