using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumns.Requests.UpdateTaskColumnByIdRequest;

public sealed record UpdateTaskColumnByIdRequest(int TaskColumnId, int? UserId, string? Title, string? Description)
    : RequestBase<UserTaskColumnDto>;
public sealed record UpdateTaskColumnByIdResponse(int TaskColumnId, int? UserId, string? Title, string? Description)
    : ResponseBase;

public sealed class UpdateTaskColumnByIdRequestHandler : RequestHandlerBase<UpdateTaskColumnByIdRequest, UserTaskColumnDto>
{
    public UpdateTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<UserTaskColumnDto> Handle(UpdateTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var taskColumnQueryResult = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id '{request.TaskColumnId}' not found");

        if (request.Title != null) taskColumnQueryResult.Title = request.Title;
        if (request.Description != null) taskColumnQueryResult.Description = request.Description;
        if (request.UserId != null)
        {
            var userQueryResult = await UnitOfWork.Users.GetByIdAsync((int)request.UserId, cancellationToken)
                ?? throw new EntityNotFoundException($"User with id '{request.UserId}' not found");

            taskColumnQueryResult.Owner = userQueryResult;
        }

        var response = taskColumnQueryResult.ToResponse();

        await UnitOfWork.UserTaskColumns.UpdateAsync(taskColumnQueryResult, cancellationToken);

        return response;
    }
}