using TaskManager.Application.Common.Requests;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTaskColumn.Requests.Commands;

public sealed record DeleteTaskColumnByIdRequest(int TaskColumnId) : RequestBase<DeleteTaskColumnByIdResponse>;
public sealed record DeleteTaskColumnByIdResponse : ResponseBase;

public sealed class DeleteTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork)
: RequestHandlerBase<DeleteTaskColumnByIdRequest, DeleteTaskColumnByIdResponse>(unitOfWork)
{
    public override async Task<DeleteTaskColumnByIdResponse> Handle(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.UserTaskColumns.FindByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new TaskColumnNotFoundException(request.TaskColumnId);

        await UnitOfWork.UserTaskColumns.DeleteAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = new DeleteTaskColumnByIdResponse();

        return response;
    }
}
