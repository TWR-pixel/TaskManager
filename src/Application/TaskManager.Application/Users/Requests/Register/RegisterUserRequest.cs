using Microsoft.Extensions.Options;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Application.Modules.Email.Sender;
using TaskManager.Application.Modules.Email.Sender.Options;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Roles.Specifications;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.Register;

public sealed record RegisterUserRequest(string Username, string Email, string Password) : RequestBase<RegisterUserResponse>;
public sealed record RegisterUserResponse() : ResponseBase;

public sealed class RegisterUserRequestHandler
    : RequestHandlerBase<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailSender _emailSender;
    private readonly EmailSenderOptions _emailOptions;

    public RegisterUserRequestHandler(IUnitOfWork unitOfWork,
                                      IPasswordHasher passwordHasher,
                                      IEmailSender emailSender,
                                      IOptions<EmailSenderOptions> emailOptions) : base(unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
        _emailOptions = emailOptions.Value;
    }

    public override async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadUserByEmailSpec(request.Email), cancellationToken);

        if (user != null)
            throw new UserAlreadyExistsException(request.Email);

        var roleEntity = await UnitOfWork.Roles.SingleOrDefaultAsync(new GetRoleByNameSpecification("User"), cancellationToken)
            ?? throw new RoleNotFoundException("User");

        var passwordSalt = _passwordHasher.GenerateSalt();
        var passwordHash = _passwordHasher.HashPassword(request.Password, passwordSalt);

        var userEntity = new UserEntity(roleEntity,
                                        request.Email,
                                        request.Username,
                                        passwordHash,
                                        passwordSalt);

        userEntity = await UnitOfWork.Users.AddAsync(userEntity, cancellationToken);

        var response = new RegisterUserResponse()
        {
            Status = "Success. Verification code has been sent to your email"
        };

        await _emailSender.SendVerificationCodeAsync(_emailOptions.From, request.Email, cancellationToken);

        #region Default columns adding
        var defaultColumns = new List<TaskColumnEntity>()
        {
            new(userEntity, "Нужно сделать"),
            new(userEntity,"В процессе"),
            new(userEntity, "Завершенные"),
        };
        #endregion

        await UnitOfWork.UserTaskColumns.AddRangeAsync(defaultColumns, cancellationToken);

        return response;
    }
}
