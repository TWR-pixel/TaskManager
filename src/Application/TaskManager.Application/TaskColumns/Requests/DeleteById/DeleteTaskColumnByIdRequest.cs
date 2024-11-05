using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Common.Exceptions;

namespace TaskManager.Application.TaskColumns.Requests.DeleteById;

public sealed record DeleteTaskColumnByIdRequest(int TaskColumnId) : RequestBase<DeleteTaskColumnByIdResponse>;
public sealed record DeleteTaskColumnByIdResponse : ResponseBase;

public sealed class DeleteTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork)
: RequestHandlerBase<DeleteTaskColumnByIdRequest, DeleteTaskColumnByIdResponse>(unitOfWork)
{
    public override async Task<DeleteTaskColumnByIdResponse> Handle(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("task column not found by id " + request.TaskColumnId);

        await UnitOfWork.UserTaskColumns.DeleteAsync(entity, cancellationToken);
        
        var response = new DeleteTaskColumnByIdResponse();

        return response;
    }
}
