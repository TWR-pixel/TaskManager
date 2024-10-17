using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Users.Requests.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Users.Requests.Common.Security.Authentication.JwtClaims;
using TaskManager.Application.Users.Requests.Common.Security.Hashers;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Roles.Specifications;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.RegisterUserRequests;

public sealed record RegisterUserRequest : RequestBase<RegisterUserResponse>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public sealed record RegisterUserResponse : ResponseBase
{
    [SetsRequiredMembers]
    public RegisterUserResponse(string accessTokenString,
                                string refreshTokenString,
                                string username,
                                int userId,
                                string roleName,
                                int roleId)
    {
        AccessTokenString = accessTokenString;
        RefreshTokenString = refreshTokenString;
        Username = username;
        UserId = userId;
        RoleName = roleName;
        RoleId = roleId;
    }

    public required string AccessTokenString { get; set; }
    public required string RefreshTokenString { get; set; }

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
    private readonly IJwtRefreshTokenGenerator _jwtRefreshTokenGenerator;

    public RegisterUserRequestHandler(IUnitOfWork unitOfWork, IJwtSecurityTokenFactory jwtTokenFactory,
                                      IPasswordHasher passwordHasher,
                                      IJwtClaimsFactory claimsFactory,
                                      IJwtRefreshTokenGenerator jwtRefreshTokenGenerator) : base(unitOfWork)
    {
        _jwtTokenFactory = jwtTokenFactory;
        _passwordHasher = passwordHasher;
        _claimsFactory = claimsFactory;
        _jwtRefreshTokenGenerator = jwtRefreshTokenGenerator;
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
        var refreshToken = _jwtRefreshTokenGenerator.GenerateRefreshToken();

        var userEntity = new UserEntity(roleEntity,
                                        request.Email,
                                        request.Username,
                                        passwordHash,
                                        passwordSalt,
                                        refreshToken);

        userEntity = await UnitOfWork.Users.AddAsync(userEntity, cancellationToken);

        var claims = _claimsFactory.CreateDefault(userEntity.Id, roleEntity.Id, userEntity.Username, roleEntity.Name);
        var token = _jwtTokenFactory.CreateSecurityToken(claims); // create new jwt token with claims

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new RegisterUserResponse(accessTokenString,
                                                _jwtRefreshTokenGenerator.GenerateRefreshToken(),
                                                userEntity.Username,
                                                userEntity.Id,
                                                userEntity.Role.Name,
                                                userEntity.Role.Id);

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
