using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries.GetById;

public sealed record GetUserByIdRequest(int UserId) : QueryRequestBase<UserDto>;

public sealed class GetUserByIdRequestHandler(IReadonlyUnitOfWork unitOfWork) : QueryRequestHandlerBase<GetUserByIdRequest, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetWithRoleByIdAsync(request.UserId, cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        var response = userEntity.ToResponse();

        return response;
    }
}
