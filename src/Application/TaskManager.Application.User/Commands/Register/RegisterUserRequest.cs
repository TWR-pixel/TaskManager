﻿using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.User.Common.AccessToken;
using TaskManager.Application.User.Common.Email.Sender;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Roles;
using TaskManager.Core.UseCases.Roles.Specifications;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.User.Commands.Register;

public sealed record RegisterUserRequest(string Username, string Email, string Password) : RequestBase<AccessTokenResponse>;
public sealed record RegisterUserResponse() : ResponseBase;

public sealed class RegisterUserRequestHandler
    : RequestHandlerBase<RegisterUserRequest, AccessTokenResponse>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailSenderService _emailSender;
    private readonly IAccessTokenFactory _tokenFactory;

    public RegisterUserRequestHandler(IUnitOfWork unitOfWork,
                                      IPasswordHasher passwordHasher,
                                      IEmailSenderService emailSender,
                                      IAccessTokenFactory tokenFactory) : base(unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
        _tokenFactory = tokenFactory;
    }

    public override async Task<AccessTokenResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users
            .SingleOrDefaultAsync(new GetUserByEmailSpec(request.Email), cancellationToken);

        if (user != null)
            throw new UserAlreadyExistsException(request.Email);

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

        //var response = new RegisterUserResponse()
        //{
        //    Status = "Success. Verification code has been sent to your email"
        //};

        //await _emailSender.SendVerificationMessageAsync(request.Email, cancellationToken);

        #region Default columns adding
        var defaultColumns = new List<TaskColumnEntity>()
        {
            new(userEntity, "Нужно сделать"),
            new(userEntity,"В процессе"),
            new(userEntity, "Завершенные"),
        };
        #endregion

        await UnitOfWork.UserTaskColumns.AddRangeAsync(defaultColumns, cancellationToken);

        var response = _tokenFactory.Create(userEntity);

        return response;
    }
}