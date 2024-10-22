using System.ComponentModel.DataAnnotations;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.GetById;

public sealed record GetUserByIdRequest([Required] int UserId) : RequestBase<GetUserByIdResponse>;
public sealed record GetUserByIdResponse(string UserName,
                                         string UserEmail,
                                         int RoleId,
                                         string RoleName) : ResponseBase;

public sealed class GetUserByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetUserByIdRequest, GetUserByIdResponse>(unitOfWork)
{
    public override async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadUserByIdSpecification(request.UserId), cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        var response = new GetUserByIdResponse(queryResult.Username,
                                               queryResult.EmailLogin,
                                               queryResult.Role.Id,
                                               queryResult.Role.Name);

        return response;
    }
}
