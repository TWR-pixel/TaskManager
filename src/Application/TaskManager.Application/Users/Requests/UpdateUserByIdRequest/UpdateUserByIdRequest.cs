using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Application.Users.Requests.UpdateUserByIdRequest;

public sealed record UpdateUserByIdRequest : RequestBase<UpdateUserByIdResponse>
{
    public required int UserId { get; set; }
    public string? Username { get; set; }
    public string? UserEmail { get; set; }
    public string? Password { get; set; }

}

public sealed record UpdateUserByIdResponse : ResponseBase
{
    [SetsRequiredMembers]
    public UpdateUserByIdResponse(int userId, string username, string userEmail)
    {
        UserId = userId;
        Username = username;
        UserEmail = userEmail;
    }

    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string UserEmail { get; set; }

}

public sealed class UpdateUserByIdRequetHandler : RequestHandlerBase<UpdateUserByIdRequest, UpdateUserByIdResponse>
{
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserByIdRequetHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : base(unitOfWork)
    {
        _passwordHasher = passwordHasher;
    }

    public override async Task<UpdateUserByIdResponse> Handle(UpdateUserByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
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

        var response = new UpdateUserByIdResponse(userEntity.Id, userEntity.Username, userEntity.EmailLogin);

        await UnitOfWork.Users.UpdateAsync(userEntity, cancellationToken);

        return response;
    }
}
