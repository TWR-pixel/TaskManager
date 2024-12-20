﻿using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTaskColumn.Requests.Commands;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
/// <param name="UserId"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnRequest(int UserId, string Title, string? Description) : RequestBase<UserTaskColumnDto>;

public sealed class CreateTaskColumnRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<CreateTaskColumnRequest, UserTaskColumnDto>(unitOfWork)
{
    public override async Task<UserTaskColumnDto> Handle(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.FindByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        var entity = new UserTaskColumnEntity(userEntity, request.Title, request.Description);

        var queryResult = await UnitOfWork.UserTaskColumns.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = queryResult.ToResponse();

        return response;
    }
}