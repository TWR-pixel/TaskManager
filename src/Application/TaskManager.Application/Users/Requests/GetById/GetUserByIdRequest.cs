using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.GetById;

public sealed record GetUserByIdRequest(int UserId) : RequestBase<UserDto>;

public sealed class GetUserByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetUserByIdRequest, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadUserByIdSpecification(request.UserId), cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        var response = queryResult.ToResponse();

        return response;
    }
}
