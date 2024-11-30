using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;

namespace TaskManager.Domain.Entities.Users;

public sealed class UserEntity : IdentityUser<int>, IEntity
{
    [SetsRequiredMembers]
    public UserEntity(RoleEntity role,
                      string emailLogin,
                      string username,
                      string profileImageLink,
                      bool isEmailConfirmed = false)
    {
        Role = role;
        EmailLogin = emailLogin;
        UserName = username;
        EmailConfirmed = isEmailConfirmed;
        ProfileImageLink = profileImageLink;
    }

    public UserEntity() { }

    [StringLength(128, MinimumLength = 3)]
    [EmailAddress]
    public string EmailLogin { get; set; }

    [StringLength(128, MinimumLength = 3)]
    public string? ProfileImageLink { get; set; }

    [StringLength(256, MinimumLength = 3)]
    public string PasswordSalt { get; set; } = "default";

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    public DateTime PasswordUpdatedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<UserTaskColumnEntity>? TaskColumns { get; set; }
    public IEnumerable<UserTaskEntity>? Tasks { get; set; }

    public RoleEntity Role { get; set; }
}
