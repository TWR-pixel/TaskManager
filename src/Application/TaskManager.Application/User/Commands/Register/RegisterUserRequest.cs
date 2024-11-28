using FluentValidation;
using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Email;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;

namespace TaskManager.Application.User.Commands.Register;

public sealed record RegisterUserRequest(string Username, string Email, string Password) : RequestBase<AccessTokenResponse>;
public sealed record RegisterUserResponse() : ResponseBase;

public sealed class RegisterUserRequestHandler(IUnitOfWork unitOfWork,
                                               IPasswordHasher passwordHasher,
                                               IEmailSender emailSender,
                                               IEmailExistingChecker emailChecker,
                                               IAccessTokenFactory tokenFactory,
                                               IValidator<RegisterUserRequest> validator,
                                               ICodeStorage codeStorage,
                                               ICodeGenerator<string> codeGenerator) : RequestHandlerBase<RegisterUserRequest, AccessTokenResponse>(unitOfWork)
{

    public override async Task<AccessTokenResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

        if (user is not null)
            throw new UserAlreadyExistsException(request.Email);

        var doesEmailExists = await emailChecker.DoesEmailExistAsync(request.Email, cancellationToken);

        if (!doesEmailExists)
            throw new EmailDoesntExistException(request.Email); ;

        var randomVerificationCode = codeGenerator.GenerateCode(20);
        codeStorage.Add(randomVerificationCode, request.Email);

        var roleEntity = await UnitOfWork.Roles.GetByNameAsync(RoleConstants.User, cancellationToken)
            ?? throw new RoleNotFoundException(RoleConstants.User);

        var passwordSalt = passwordHasher.GenerateSalt();
        var passwordHash = passwordHasher.HashPassword(request.Password, passwordSalt);

        var userEntity = new UserEntity(roleEntity,
                                        request.Email,
                                        request.Username,
                                        passwordHash,
                                        passwordSalt,
                                        string.Empty);

        userEntity = await UnitOfWork.Users.AddAsync(userEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var defaultColumns = new List<UserTaskColumnEntity>()
        {
            new(userEntity, "Нужно сделать"),
            new(userEntity,"В процессе"),
            new(userEntity, "Завершенные"),
        };

        await UnitOfWork.UserTaskColumns.AddRangeAsync(defaultColumns, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = tokenFactory.Create(userEntity);

        return response;
    }
}
