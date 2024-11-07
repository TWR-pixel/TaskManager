using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumn.Requests.UpdateById;

public sealed record UpdateTaskColumnByIdRequest(int TaskColumnId,
                                                 int? UserId,
                                                 string? Name,
                                                 string? Description)
    : RequestBase<UpdateTaskColumnByIdResponse>;

public sealed record UpdateTaskColumnByIdResponse(int? UserId, string? Name, string? Description)
    : ResponseBase;

#region Handler
public sealed class UpdateTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<UpdateTaskColumnByIdRequest, UpdateTaskColumnByIdResponse>(unitOfWork)
{
    public override async Task<UpdateTaskColumnByIdResponse> Handle(UpdateTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var taskColumnEntity = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException($"Task column with id '{request.TaskColumnId}' not found");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            taskColumnEntity.Title = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            taskColumnEntity.Description = request.Description;
        }

        if (request.UserId is not null)
        {
            var userEntity = await UnitOfWork.Users.GetByIdAsync((int)request.UserId, cancellationToken)
                ?? throw new UserNotFoundException((int)request.UserId);

            taskColumnEntity.Owner = userEntity;
        }

        var response = new UpdateTaskColumnByIdResponse(taskColumnEntity.Owner.Id, taskColumnEntity.Title, taskColumnEntity.Description);

        await UnitOfWork.UserTaskColumns.UpdateAsync(taskColumnEntity, cancellationToken);

        return response;
    }
}
#endregion