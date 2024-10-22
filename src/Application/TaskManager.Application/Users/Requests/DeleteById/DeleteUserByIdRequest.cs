using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.DeleteById;

public sealed record DeleteUserByIdRequest(int UserId) : RequestBase<DeleteUserByIdResponse>;
public sealed record DeleteUserByIdResponse : ResponseBase;

public sealed class DeleteUserByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<DeleteUserByIdRequest, DeleteUserByIdResponse>(unitOfWork)
{
    public override async Task<DeleteUserByIdResponse> Handle(DeleteUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException($"User with id '{request.UserId}' not found. ");

        await UnitOfWork.Users.DeleteAsync(queryResult, cancellationToken);

        var response = new DeleteUserByIdResponse();

        return response;
    }
}
