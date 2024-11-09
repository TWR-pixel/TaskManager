using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands.UpdateById;

public sealed record UpdateUserByIdRequest(int UserId,
                                           string? Username = null,
                                           string? UserEmail = null,
                                           string? CurrentPassword = null,
                                           string? NewPassword = null) : RequestBase<UpdateUserByIdResponse>;
public sealed record UpdateUserByIdResponse(int UserId, string Username, string UserEmail) : ResponseBase;

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
            ?? throw new UserNotFoundException(request.UserId);

        if (request.Username != null)
            userEntity.Username = request.Username;

        if (request.UserEmail != null)
            userEntity.EmailLogin = request.UserEmail;

        if (request.NewPassword != null && request.CurrentPassword != null)
        {
            var currentPasswordHash = _passwordHasher.HashPassword(request.CurrentPassword, userEntity.PasswordSalt);

            if (currentPasswordHash != userEntity.PasswordHash)
                throw new NotRightPasswordException(request.CurrentPassword);

            var salt = _passwordHasher.GenerateSalt();
            var passwordHash = _passwordHasher.HashPassword(request.NewPassword, salt);

            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = salt;

            userEntity.PasswordUpdatedAt = DateTime.UtcNow;
        }

        await UnitOfWork.Users.UpdateAsync(userEntity, cancellationToken);

        var response = new UpdateUserByIdResponse(userEntity.Id, userEntity.Username, userEntity.EmailLogin);

        return response;
    }
}
