using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record GetUserByIdQuery(int UserId) : QueryBase<UserDto>;

public sealed class GetUserByIdQueryHandler(IReadonlyUnitOfWork unitOfWork) : QueryHandlerBase<GetUserByIdQuery, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetWithRoleByIdAsync(request.UserId, cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        var response = userEntity.ToResponse();

        return response;
    }
}
