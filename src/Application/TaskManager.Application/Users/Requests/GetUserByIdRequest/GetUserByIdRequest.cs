using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.GetUserByIdRequest;

public sealed class GetUserByIdRequest : RequestBase<GetUserByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed class GetUserByIdResponse : ResponseBase
{
    public required string UserName { get; set; }
    public required string UserEmail { get; set; }
    public required string Username { get; set; }
}

public sealed class GetUserByIdRequestHandler(EfRepositoryBase<UserEntity> usersRepo) : RequestHandlerBase<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo = usersRepo;

    public override async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _usersRepo.SingleOrDefaultAsync(new GetUserByIdSpecification(request.UserId), cancellationToken)
            ?? throw new EntityNotFoundException("User not found by id = " + request.UserId);

        var response = new GetUserByIdResponse
        {
            UserEmail = queryResult.EmailLogin,
            Username = queryResult.Username,
            UserName = queryResult.Username,
        };

        return response;
    }
}
