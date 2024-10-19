using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.UpdateById;

public sealed record UpdateUserByIdRequest(int UserId,
                                           string? Username = null,
                                           string? UserEmail = null,
                                           string? Password = null) : RequestBase<UpdateUserByIdResponse>;
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
