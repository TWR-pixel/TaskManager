using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Application.Common.Services.EmailSender;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Roles.Specifications;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.Register;

public sealed record RegisterUserRequest(string Username, string Email, string Password) : RequestBase<RegisterUserResponse>;
public sealed record RegisterUserResponse() : ResponseBase
{
    //[SetsRequiredMembers]
    //public RegisterUserResponse(
    //                            string username,
    //                            int userId,
    //                            string roleName,
    //                            int roleId)
    //{
    //    Username = username;
    //    UserId = userId;
    //    RoleName = roleName;
    //    RoleId = roleId;
    //}

    //public required string Username { get; set; }
    //public required int UserId { get; set; }

    //public required string RoleName { get; set; }
    //public required int RoleId { get; set; }
}

public sealed class RegisterUserRequestHandler
    : RequestHandlerBase<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailSender _emailSender;

    public RegisterUserRequestHandler(IUnitOfWork unitOfWork,
                                      IPasswordHasher passwordHasher,
                                      IEmailSender emailSender) : base(unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
    }

    public override async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadUserByEmailSpecification(request.Email), cancellationToken);

        if (user != null)
            throw new UserAlreadyExistsException($"User with email '{request.Email}' already exists");

        var roleEntity = await UnitOfWork.Roles.SingleOrDefaultAsync(new GetRoleByNameSpecification("User"), cancellationToken)
            ?? throw new EntityNotFoundException($"Role with name 'User' not found");

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
            Status = "Success, verify your email"
        };

        await _emailSender.SendVerificationCodeAsync(request.Email, userEntity, cancellationToken);

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
