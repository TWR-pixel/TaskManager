﻿using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTaskColumn.Requests.Commands;

public sealed record UpdateTaskColumnByIdRequest(int TaskColumnId,
                                                 int? UserId,
                                                 string? Title,
                                                 string? Description)
    : RequestBase<UpdateTaskColumnByIdResponse>;

public sealed record UpdateTaskColumnByIdResponse(int? UserId, string? Title, string? Description)
    : ResponseBase;

#region Handler
public sealed class UpdateTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<UpdateTaskColumnByIdRequest, UpdateTaskColumnByIdResponse>(unitOfWork)
{
    public override async Task<UpdateTaskColumnByIdResponse> Handle(UpdateTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var taskColumnEntity = await UnitOfWork.UserTaskColumns.FindByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id '{request.TaskColumnId}' not found");

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            taskColumnEntity.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            taskColumnEntity.Description = request.Description;
        }

        if (request.UserId is not null)
        {
            var userEntity = await UnitOfWork.Users.FindByIdAsync((int)request.UserId, cancellationToken)
                ?? throw new UserNotFoundException((int)request.UserId);

            taskColumnEntity.Owner = userEntity;
        }

        var response = new UpdateTaskColumnByIdResponse(taskColumnEntity.Owner.Id,
                                                        taskColumnEntity.Title,
                                                        taskColumnEntity.Description);

        await UnitOfWork.UserTaskColumns.UpdateAsync(taskColumnEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return response;
    }
}
#endregion