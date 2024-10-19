using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumns.Requests.UpdateById;

public sealed record UpdateTaskColumnByIdRequest(int TaskColumnId, int? UserId, string? Name, string? Description)
    : RequestBase<UpdateTaskColumnByIdResponse>;
public sealed record UpdateTaskColumnByIdResponse(int? UserId, string? Name, string? Description)
    : ResponseBase;

public sealed class UpdateTaskColumnByIdRequestHandler : RequestHandlerBase<UpdateTaskColumnByIdRequest, UpdateTaskColumnByIdResponse>
{
    public UpdateTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<UpdateTaskColumnByIdResponse> Handle(UpdateTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var taskColumnQueryResult = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id '{request.TaskColumnId}' not found");

        if (request.Name != null) taskColumnQueryResult.Name = request.Name;
        if (request.Description != null) taskColumnQueryResult.Description = request.Description;
        if (request.UserId != null)
        {
            var userQueryResult = await UnitOfWork.Users.GetByIdAsync((int)request.UserId, cancellationToken)
                ?? throw new EntityNotFoundException($"User with id '{request.UserId}' not found");

            taskColumnQueryResult.Owner = userQueryResult;
        }


        var response = new UpdateTaskColumnByIdResponse(taskColumnQueryResult.Owner.Id, taskColumnQueryResult.Name, taskColumnQueryResult.Description);

        await UnitOfWork.UserTaskColumns.UpdateAsync(taskColumnQueryResult, cancellationToken);

        return response;
    }
}