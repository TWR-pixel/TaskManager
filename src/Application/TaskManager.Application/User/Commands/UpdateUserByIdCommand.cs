using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.Common.Security;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed record UpdateUserByIdCommand(int UserId,
                                           string? Username = null,
                                           string? UserEmail = null,
                                           string? CurrentPassword = null,
                                           string? NewPassword = null) : CommandBase<UpdateUserByIdResponse>;
public sealed record UpdateUserByIdResponse(int UserId, string Username, string UserEmail) : ResponseBase;

public sealed class UpdateUserByIdCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : CommandHandlerBase<UpdateUserByIdCommand, UpdateUserByIdResponse>(unitOfWork)
{
    public override async Task<UpdateUserByIdResponse> Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        if (request.Username != null)
            userEntity.UserName = request.Username;

        if (request.UserEmail != null)
            userEntity.EmailLogin = request.UserEmail;

        if (request.NewPassword != null && request.CurrentPassword != null)
        {
            var currentPasswordHash = passwordHasher.HashPassword(request.CurrentPassword, userEntity.PasswordSalt);

            if (currentPasswordHash != userEntity.PasswordHash)
                throw new NotRightPasswordException(request.CurrentPassword);

            var salt = passwordHasher.GenerateSalt();
            var passwordHash = passwordHasher.HashPassword(request.NewPassword, salt);

            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = salt;

            userEntity.PasswordUpdatedAt = DateTime.UtcNow;
        }

        await UnitOfWork.Users.UpdateAsync(userEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = new UpdateUserByIdResponse(userEntity.Id, userEntity.UserName, userEntity.EmailLogin);

        return response;
    }
}
