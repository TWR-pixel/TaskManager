using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TaskManager.Application.Common.Code;
using TaskManager.Application.Common.Email;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;

namespace TaskManager.Application.User.Commands;

public sealed record RegisterUserWithJwtBearerSchemeCommand(string Username, string Email, string Password) : CommandBase<AccessTokenResponse>;
public sealed record RegisterUserResponse() : ResponseBase;

public sealed class RegisterUserWithJwtBearerScheme(IUnitOfWork unitOfWork,
                                               IPasswordHasher passwordHasher,
                                               IEmailSender emailSender,
                                               IEmailExistingChecker emailChecker,
                                               IAccessTokenFactory tokenFactory,
                                               IValidator<RegisterUserWithJwtBearerSchemeCommand> validator,
                                               ICodeStorage codeStorage,
                                               ICodeGenerator<string> codeGenerator,
                                               UserManager<UserEntity> userManager) : CommandHandlerBase<RegisterUserWithJwtBearerSchemeCommand, AccessTokenResponse>(unitOfWork)
{

    public override async Task<AccessTokenResponse> Handle(RegisterUserWithJwtBearerSchemeCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

        if (user is not null)
            throw new UserAlreadyExistsException(request.Email);

        var doesEmailExists = await emailChecker.DoesEmailExistAsync(request.Email, cancellationToken);

        if (!doesEmailExists)
            throw new EmailDoesntExistException(request.Email);

        var roleEntity = await UnitOfWork.Roles.GetByNameAsync(RoleConstants.User, cancellationToken)
            ?? throw new RoleNotFoundException(RoleConstants.User);

        var passwordSalt = passwordHasher.GenerateSalt();
        var passwordHash = passwordHasher.HashPassword(request.Password, passwordSalt);

        var userEntity = new UserEntity(roleEntity,
                                        request.Email,
                                        request.Username,
                                        string.Empty);

        var randomVerificationCode = codeGenerator.GenerateCode(20);
        codeStorage.Add(randomVerificationCode, request.Email);

        await userManager.CreateAsync(userEntity, request.Password);
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
