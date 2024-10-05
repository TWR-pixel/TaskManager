using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Authentication;
using TaskManager.Application.Common.Security.Authentication.Abstractions;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.Data.Role.Specifications;
using TaskManager.Data.User.Specifications;

namespace TaskManager.Application.Users.Requests.RegisterUserRequests;

public sealed class RegisterUserRequest : RequestBase<RegisterUserResponse>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public sealed class RegisterUserResponse : ResponseBase
{
    public required string TokenString { get; set; }

    public required string Username { get; set; }
    public required int UserId { get; set; }

    public required string RoleName { get; set; }
    public required int RoleId { get; set; }
}

public sealed class RegisterUserRequestHandler
    : RequestHandlerBase<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IJwtSecurityTokenFactory _jwtTokenFactory;
    private readonly IBCryptPasswordHasher _passwordHasher;
    private readonly IJwtClaimsFactory _claimsFactory;

    private readonly EfRepositoryBase<RoleEntity> _roleRepo;
    private readonly EfRepositoryBase<UserEntity> _userRepo;

    public RegisterUserRequestHandler(IJwtSecurityTokenFactory jwtTokenFactory, IBCryptPasswordHasher passwordHasher,
        EfRepositoryBase<RoleEntity> roleRepo, EfRepositoryBase<UserEntity> userRepo, IJwtClaimsFactory claimsFactory)
    {
        _jwtTokenFactory = jwtTokenFactory;
        _passwordHasher = passwordHasher;
        _roleRepo = roleRepo;
        _userRepo = userRepo;
        _claimsFactory = claimsFactory;
    }

    public override async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepo
            .SingleOrDefaultAsync(new GetUserByEmailLoginSpecification(request.Email), cancellationToken);

        if (user != null)
            throw new UserAlreadyExistsException($"User with email = {request.Email} already exists");

        var roleEntity = await _roleRepo.SingleOrDefaultAsync(new GetRoleByNameSpecification("User"), cancellationToken)
            ?? throw new EntityNotFoundException($"Entity not found by name 'User'");

        var passwordSalt = _passwordHasher.GenerateSalt();
        var passwordHash = _passwordHasher.HashPassword(request.Password, passwordSalt);

        var userEntity = new UserEntity()
        {
            EmailLogin = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = roleEntity,
            Username = request.Username
        };

        userEntity = await _userRepo.AddAsync(userEntity, cancellationToken);

        var claims = _claimsFactory.CreateDefault(userEntity.Id, roleEntity.Id, userEntity.Username, roleEntity.Name);
        var token = _jwtTokenFactory.CreateSecurityToken(claims); // create new jwt token with claims

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new RegisterUserResponse
        {
            TokenString = tokenString,
            Username = request.Username,
            UserId = userEntity.Id,
            RoleId = roleEntity.Id,
            RoleName = roleEntity.Name,
        };

        return response;
    }
}
