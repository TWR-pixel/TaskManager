using FluentValidation;
using MailerooClient.Email.Verification;
using MailerooClient.Email.Verification.Requests.Check;
using Microsoft.Extensions.Options;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.AccessToken;
using TaskManager.Application.User.Common.Email.Sender;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.Roles.Specifications;
using TaskManager.Domain.UseCases.Users.Specifications;

namespace TaskManager.Application.User.Commands.Register;

public sealed record RegisterUserRequest(string Username, string Email, string Password) : RequestBase<AccessTokenResponse>;
public sealed record RegisterUserResponse() : ResponseBase;

public sealed class RegisterUserRequestHandler(IUnitOfWork unitOfWork,
                                  IPasswordHasher passwordHasher,
                                  IEmailSenderService emailSender,
                                  IAccessTokenFactory tokenFactory,
                                  IValidator<RegisterUserRequest> validator) : RequestHandlerBase<RegisterUserRequest, AccessTokenResponse>(unitOfWork)
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly IAccessTokenFactory _tokenFactory = tokenFactory;
    private readonly MailerooClientOptions _options = new(EnvironmentWrapper.GetEnvironmentVariable("TM_MAILEROO_API_KEY") ?? throw new NullReferenceException("TM_MAILEROO_API_KEY doesnt exists")); // because from IOptions<> interface doesnt work
    private readonly IValidator<RegisterUserRequest> _validator = validator;

    public override async Task<AccessTokenResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await UnitOfWork.Users
            .SingleOrDefaultAsync(new GetUserByEmailSpec(request.Email), cancellationToken);

        if (user != null)
            throw new UserAlreadyExistsException(request.Email);

        var checkEmail = new CheckRequest(request.Email);
        using var client = new MailerooApiClient(_options);
        var checkResponse = await client.SendRequestAsync(checkEmail, cancellationToken);

        if (checkResponse.Data is null)
            throw new HttpRequestException();

        if (!checkResponse.Data.MxFound) // This means if the mail does not exist
            throw new MxNotFoundException(request.Email);

        var userRole = RoleConstants.USER;
        var roleEntity = await UnitOfWork.Roles.SingleOrDefaultAsync(new GetRoleByNameSpec(userRole), cancellationToken)
            ?? throw new RoleNotFoundException(userRole);

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

        #region Default columns adding
        var defaultColumns = new List<UserTaskColumnEntity>()
            {
                new(userEntity, "Нужно сделать", 1),
                new(userEntity,"В процессе", 2),
                new(userEntity, "Завершенные", 3),
            };
        #endregion

        await UnitOfWork.UserTaskColumns.AddRangeAsync(defaultColumns, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = _tokenFactory.Create(userEntity);


        return response;
    }
}
