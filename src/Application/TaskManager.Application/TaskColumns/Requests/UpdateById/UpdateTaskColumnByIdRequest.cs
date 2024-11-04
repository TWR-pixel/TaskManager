using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;

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

        if (request.Name != null) taskColumnQueryResult.Title = request.Name;
        if (request.Description != null) taskColumnQueryResult.Description = request.Description;
        if (request.UserId != null)
        {
            var userQueryResult = await UnitOfWork.Users.GetByIdAsync((int)request.UserId, cancellationToken)
                ?? throw new UserNotFoundException((int)request.UserId);

            taskColumnQueryResult.Owner = userQueryResult;
        }


        var response = new UpdateTaskColumnByIdResponse(taskColumnQueryResult.Owner.Id, taskColumnQueryResult.Title, taskColumnQueryResult.Description);

        await UnitOfWork.UserTaskColumns.UpdateAsync(taskColumnQueryResult, cancellationToken);

        return response;
    }
}