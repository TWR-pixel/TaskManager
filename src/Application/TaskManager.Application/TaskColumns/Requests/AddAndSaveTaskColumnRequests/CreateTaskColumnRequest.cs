﻿using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.TaskColumns.Requests.AddAndSaveTaskColumnRequests;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
public sealed class CreateTaskColumnRequest : RequestBase<CreateTaskColumnResponse>
{
    public required int UserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Response for user
/// </summary>
public sealed class CreateTaskColumnResponse : ResponseBase
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public sealed class CreateTaskColumnRequestHandler :
    RequestHandlerBase<CreateTaskColumnRequest, CreateTaskColumnResponse>
{
    private readonly EfRepositoryBase<TaskColumnEntity> _taskColumnRepo;
    private readonly EfRepositoryBase<UserEntity> _usersRepo;

    public CreateTaskColumnRequestHandler(EfRepositoryBase<TaskColumnEntity> taskColumnRepo, EfRepositoryBase<UserEntity> usersRepo)
    {
        _taskColumnRepo = taskColumnRepo;
        _usersRepo = usersRepo;
    }

    public override async Task<CreateTaskColumnResponse> Handle
        (CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await _usersRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("user not found by id " + request.UserId);

        var entity = new TaskColumnEntity
        {
            Name = request.Name,
            Description = request.Description,
            Owner = userEntity
        };

        var queryResult = await _taskColumnRepo.AddAsync(entity, cancellationToken);

        var response = new CreateTaskColumnResponse
        {
            Id = queryResult.Id,
            Name = queryResult.Name,
            Description = queryResult.Description
        };

        return response;
    }
}