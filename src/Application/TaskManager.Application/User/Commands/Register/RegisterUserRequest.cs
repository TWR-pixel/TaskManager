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
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAccessTokenFactory _tokenFactory = tokenFactory;
    private readonly IValidator<RegisterUserRequest> _validator = validator;
    private readonly IEmailExistingChecker _emailChecker = emailChecker;
    private readonly ICodeStorage _codeStorage = codeStorage;
    private readonly ICodeGenerator<string> _codeGenerator = codeGenerator;

    public override async Task<AccessTokenResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await UnitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

        if (user is not null)
            throw new UserAlreadyExistsException(request.Email);

        var doesEmailExists = await _emailChecker.DoesEmailExistAsync(request.Email, cancellationToken);

        if (!doesEmailExists)
            throw new EmailDoesntExistException(request.Email); ;

        var randomVerificationCode = _codeGenerator.GenerateCode(20);
        _codeStorage.Add(randomVerificationCode, request.Email);

        var roleEntity = await UnitOfWork.Roles.GetByNameAsync(RoleConstants.User, cancellationToken)
            ?? throw new RoleNotFoundException(RoleConstants.User);
        
        var passwordSalt = _passwordHasher.GenerateSalt();
        var passwordHash = _passwordHasher.HashPassword(request.Password, passwordSalt);

        var userEntity = new UserEntity(roleEntity,
                                        request.Email,
                                        request.Username,
                                        passwordHash,
                                        passwordSalt);

        userEntity = await UnitOfWork.Users.AddAsync(userEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        //var response = new RegisterUserResponse()
        //{
        //    Status = "Success. Verification code has been sent to your email"
        //};
        //await _emailSender.SendVerificationMessageAsync(request.Email, cancellationToken);

        var defaultColumns = new List<UserTaskColumnEntity>()
        {
            new(userEntity, "Нужно сделать"),
            new(userEntity,"В процессе"),
            new(userEntity, "Завершенные"),
        };

        await UnitOfWork.UserTaskColumns.AddRangeAsync(defaultColumns, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        
        var response = _tokenFactory.Create(userEntity);

        return response;
    }
}
