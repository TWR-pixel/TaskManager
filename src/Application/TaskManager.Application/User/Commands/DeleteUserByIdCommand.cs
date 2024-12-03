using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed class DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork) : CommandHandlerBase<DeleteByIdCommandBase<UserDto>, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(DeleteByIdCommandBase<UserDto> request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.GetWithRoleByIdAsync(request.Id, cancellationToken)
            ?? throw new UserNotFoundException($"User with id '{request.Id}' not found. ");

        await UnitOfWork.Users.DeleteAsync(queryResult, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = queryResult.ToResponse();

        return response;
    }
}
