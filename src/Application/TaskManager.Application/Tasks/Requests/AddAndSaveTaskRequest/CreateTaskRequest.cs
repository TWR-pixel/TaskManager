﻿using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Tasks.Requests.AddAndSaveTaskRequest;
 
public sealed class CreateTaskRequest : RequestBase<CreateTaskResponse>
{
    public required int UserId { get; set; }
    public required int ColumnId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsInProgress { get; set; } = true;
    public DateTime? DoTo { get; set; } 

}

public sealed class CreateTaskResponse : ResponseBase
{
    public required int CreatedTaskId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}

public sealed class CreateTaskRequestHandler : RequestHandlerBase<CreateTaskRequest, CreateTaskResponse>
{
    private readonly EfRepositoryBase<UserTaskEntity> _taskRepo;
    private readonly EfRepositoryBase<UserEntity> _userRepo;
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnsRepo;

    public CreateTaskRequestHandler(EfRepositoryBase<UserTaskEntity> taskRepo,
                                        EfRepositoryBase<UserEntity> userRepo,
                                        EfRepositoryBase<TaskColumnEntity> taskColumnsRepo)
    {
        _taskRepo = taskRepo;
        _userRepo = userRepo;
        _taskColumnsRepo = taskColumnsRepo;
    }

    public override async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("User not found by id " + request.UserId);

        var column = await _taskColumnsRepo.GetByIdAsync(request.ColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("Column not found by id " + request.ColumnId);

        var taskEntity = new UserTaskEntity
        {
            Title = request.Title,
            Content = request.Content,
            IsCompleted = request.IsCompleted,
            IsInProgress = request.IsInProgress,
            TaskColumn = column,
            Owner = user,
            DoTo = request.DoTo
        };

        var queryResult = await _taskRepo.AddAsync(taskEntity, cancellationToken);

        var response = new CreateTaskResponse
        {
            CreatedTaskId = queryResult.Id,
            Content = queryResult.Content,
            Title = queryResult.Title,
        };

        return response;
    }
}
