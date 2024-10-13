using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;

public sealed record DeleteTaskColumnByIdRequest : RequestBase<DeleteTaskColumnByIdResponse>
{
    public required int TaskColumnId { get; set; }
}

public sealed record DeleteTaskColumnByIdResponse : ResponseBase
{
}

public sealed class DeleteTaskColumnByIdRequestHandler
: RequestHandlerBase<DeleteTaskColumnByIdRequest, DeleteTaskColumnByIdResponse>
{
    public DeleteTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<DeleteTaskColumnByIdResponse> Handle(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("task column not found by id " + request.TaskColumnId);

        await UnitOfWork.UserTaskColumns.DeleteAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = new DeleteTaskColumnByIdResponse();

        return response;
    }
}
