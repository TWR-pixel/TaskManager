using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;

public sealed record DeleteTaskColumnByIdRequest : RequestBase<UserTaskColumnDto>
{
    public required int TaskColumnId { get; set; }
}

public sealed record DeleteTaskColumnByIdResponse : ResponseBase
{
}

public sealed class DeleteTaskColumnByIdRequestHandler(IUnitOfWork unitOfWork)
: RequestHandlerBase<DeleteTaskColumnByIdRequest, UserTaskColumnDto>(unitOfWork)
{
    public override async Task<UserTaskColumnDto> Handle(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.UserTaskColumns.GetByIdAsync(request.TaskColumnId, cancellationToken)
            ?? throw new EntityNotFoundException("task column not found by id " + request.TaskColumnId);

        await UnitOfWork.UserTaskColumns.DeleteAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = entity.ToResponse();

        return response;
    }
}
