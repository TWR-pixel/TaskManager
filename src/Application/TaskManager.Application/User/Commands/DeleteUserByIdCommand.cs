using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed record DeleteUserByIdCommand(int UserId) : CommandBase<UserDto>;

public sealed class DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork) : CommandHandlerBase<DeleteUserByIdCommand, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException($"User with id '{request.UserId}' not found. ");

        await UnitOfWork.Users.DeleteAsync(queryResult, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = queryResult.ToResponse();

        return response;
    }
}
