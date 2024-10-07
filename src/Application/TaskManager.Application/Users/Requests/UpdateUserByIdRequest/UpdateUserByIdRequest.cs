using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication.Abstractions;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Users.Requests.UpdateUserByIdRequest;

public sealed class UpdateUserByIdRequest : RequestBase<UpdateUserByIdResponse>
{
    public required int UserId { get; set; }
    public string? Username { get; set; }
    public string? UserEmail { get; set; }
    public string? Password { get; set; }

}

public sealed class UpdateUserByIdResponse : ResponseBase
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string UserEmail { get; set; }
}

public sealed class UpdateUserByIdRequetHandler : RequestHandlerBase<UpdateUserByIdRequest, UpdateUserByIdResponse>
{
    private readonly EfRepositoryBase<UserEntity> _usersRepo;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserByIdRequetHandler(EfRepositoryBase<UserEntity> usersRepo, IPasswordHasher passwordHasher)
    {
        _usersRepo = usersRepo;
        _passwordHasher = passwordHasher;
    }

    public override async Task<UpdateUserByIdResponse> Handle(UpdateUserByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await _usersRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        if (request.Username != null)
            userEntity.Username = request.Username;

        if (request.UserEmail != null)
            userEntity.EmailLogin = request.UserEmail;

        if (request.Password != null)
        {
            var salt = _passwordHasher.GenerateSalt();
            var passwordHash = _passwordHasher.HashPassword(request.Password, salt);

            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = salt;
        }

        var response = new UpdateUserByIdResponse
        {
            UserEmail = userEntity.EmailLogin,
            UserId = request.UserId,
            Username = userEntity.Username,
        };

        await _usersRepo.UpdateAsync(userEntity, cancellationToken);

        return response;
    }
}
