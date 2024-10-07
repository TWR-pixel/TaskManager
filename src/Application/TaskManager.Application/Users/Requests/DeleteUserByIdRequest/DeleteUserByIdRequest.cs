using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Users.Requests.DeleteUserByIdRequest;

public sealed class DeleteUserByIdRequest : RequestBase<DeleteUserByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed class DeleteUserByIdResponse : ResponseBase
{
}

public sealed class DeleteUserByIdRequestHandler : RequestHandlerBase<DeleteUserByIdRequest, DeleteUserByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _userRepo;

    public DeleteUserByIdRequestHandler(EfRepositoryBase<UserEntity> userRepo)
    {
        _userRepo = userRepo;
    }

    public override async Task<DeleteUserByIdResponse> Handle(DeleteUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found. ");

        await _userRepo.DeleteAsync(queryResult, cancellationToken);

        var response = new DeleteUserByIdResponse();

        return response;
    }
}
