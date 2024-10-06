using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Users.Requests;

public sealed class GetUserByIdRequest : RequestBase<GetUserByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed class GetUserByIdResponse : ResponseBase
{
}

public sealed class GetUserByIdRequestHandler : RequestHandlerBase<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo;
    public GetUserByIdRequestHandler(EfRepositoryBase<UserEntity> usersRepo)
    {
        _usersRepo = usersRepo;
    }

    public override async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _usersRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("User not found by id = " + request.UserId);

        var response = new GetUserByIdResponse();

        return response;
    }
}
