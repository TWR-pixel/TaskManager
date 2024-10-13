using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.Users.Specifications;

namespace TaskManager.Application.Users.Requests.GetUserByIdRequest;

public sealed record GetUserByIdRequest : RequestBase<GetUserByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed record GetUserByIdResponse(string UserName,
                                         string UserEmail,
                                         int RoleId,
                                         string RoleName) : ResponseBase;

public sealed class GetUserByIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetUserByIdRequest, GetUserByIdResponse>(unitOfWork)
{
    public override async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new GetUserByIdSpecification(request.UserId), cancellationToken)
                ?? throw new EntityNotFoundException($"User with id '{request.UserId}' not found");
        
        var response = new GetUserByIdResponse(queryResult.Username,
                                               queryResult.EmailLogin,
                                               queryResult.Role.Id,
                                               queryResult.Role.Name);

        return response;
    }
}
