using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Users.Exceptions;

namespace TaskManager.Application.Users.Requests.DeleteById;

public sealed record DeleteUserByIdRequest(int UserId) : RequestBase<UserDto>;

public sealed class DeleteUserByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<DeleteUserByIdRequest, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(DeleteUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException($"User with id '{request.UserId}' not found. ");

        await UnitOfWork.Users.DeleteAsync(queryResult, cancellationToken);

        var response = queryResult.ToResponse();

        return response;
    }
}
