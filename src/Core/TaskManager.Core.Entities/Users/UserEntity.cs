using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.UserOrganization;

namespace TaskManager.Domain.Entities.Users;

public sealed class UserEntity : IdentityUser<int>, IEntity
{
    [SetsRequiredMembers]
    public UserEntity(RoleEntity role,
                      string emailLogin,
                      string username,
                      string profileImageLink,
                      string passwordHash = "",
                      string passwordSalt = "",
                      bool isEmailConfirmed = false)
    {
        Role = role;
        EmailLogin = emailLogin;
        UserName = username;
        EmailConfirmed = isEmailConfirmed;
        ProfileImageLink = profileImageLink;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public UserEntity() { }

    [StringLength(128, MinimumLength = 3)]
    [EmailAddress]
    public string EmailLogin { get; set; }

    [StringLength(128, MinimumLength = 3)]
    public string? ProfileImageLink { get; set; }

    public string AuthenticationScheme { get; set; } = DefaultAuthenticationScheme.AuthenticationScheme;

    [StringLength(256, MinimumLength = 3)]
    public required string PasswordSalt { get; set; } = "default";

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    public DateTime PasswordUpdatedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<UserTaskColumnEntity>? UserTaskColumns { get; set; }
    public IEnumerable<UserTaskEntity>? UserTasks { get; set; }
    public IEnumerable<UserOrganizationEntity>? UserOrganizations { get; set; }

    public RoleEntity Role { get; set; }
}
