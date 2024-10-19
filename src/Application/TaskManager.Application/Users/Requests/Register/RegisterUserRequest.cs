using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Cryptography;
using TaskManager.Application.Common.EmailSender;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Roles.Specifications;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.Register;

public sealed record RegisterUserRequest(string Username, string Email, string Password) : RequestBase<RegisterUserResponse>;
public sealed record RegisterUserResponse : ResponseBase
{
    [SetsRequiredMembers]
    public RegisterUserResponse(string accessTokenString,
                                string username,
                                int userId,
                                string roleName,
                                int roleId)
    {
        AccessTokenString = accessTokenString;
        Username = username;
        UserId = userId;
        RoleName = roleName;
        RoleId = roleId;
    }

    public required string AccessTokenString { get; set; }

    public required string Username { get; set; }
    public required int UserId { get; set; }

    public required string RoleName { get; set; }
    public required int RoleId { get; set; }
}

public sealed class RegisterUserRequestHandler
    : RequestHandlerBase<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtTokenFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtClaimsFactory _claimsFactory;
    private readonly IEmailSender _emailSender;
    private readonly IMemoryCache _cache;

    public RegisterUserRequestHandler(IUnitOfWork unitOfWork, IJwtSecurityTokenFactory jwtTokenFactory,
                                      IPasswordHasher passwordHasher,
                                      IJwtClaimsFactory claimsFactory,
                                      IEmailSender emailSender,
                                      IMemoryCache cache) : base(unitOfWork)
    {
        _jwtTokenFactory = jwtTokenFactory;
        _passwordHasher = passwordHasher;
        _claimsFactory = claimsFactory;
        _emailSender = emailSender;
        _cache = cache;
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

        var claims = _claimsFactory.CreateDefault(userEntity.Id, roleEntity.Id, userEntity.Username, roleEntity.Name);
        var token = _jwtTokenFactory.CreateSecurityToken(claims); // create new jwt token with claims

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new RegisterUserResponse(accessTokenString,
                                                userEntity.Username,
                                                userEntity.Id,
                                                userEntity.Role.Name,
                                                userEntity.Role.Id);

        var emailCode = RandomNumberGenerator.GetHexString(6, true);

        var options = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(7), // 7 minutes alive
            Priority = CacheItemPriority.High
        };

        _cache.Set(userEntity.Id.ToString(), emailCode, options);

        var emailMsg = new MailMessage(_emailSender.Options.From,
                                       request.Email,
                                       "Confirm registration",
                                       emailCode);

        await _emailSender.SendMailAsync(emailMsg, cancellationToken);

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
