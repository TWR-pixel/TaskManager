using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;

namespace TaskManager.Domain.Entities.Users;

public sealed class UserEntity : EntityBase
{
    [SetsRequiredMembers]
    public UserEntity(RoleEntity role,
                      string emailLogin,
                      string username,
                      string passwordHash,
                      string passwordSalt,
                      string profileImageLink,
                      bool isEmailConfirmed = false)
    {
        Role = role;
        EmailLogin = emailLogin;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        IsEmailVerified = isEmailConfirmed;
        ProfileImageLink = profileImageLink;
    }

    public UserEntity() { }

    [StringLength(128, MinimumLength = 3)]
    [EmailAddress]
    public required string EmailLogin { get; set; }

    [StringLength(128, MinimumLength = 3)]
    public required string Username { get; set; }
    public string? ProfileImageLink { get; set; }

    [StringLength(256, MinimumLength = 3)]
    public required string PasswordHash { get; set; }

    [StringLength(256, MinimumLength = 3)]
    public required string PasswordSalt { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public DateTime PasswordUpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsEmailVerified { get; set; } = true;

    public IEnumerable<UserTaskColumnEntity>? TaskColumns { get; set; }
    public IEnumerable<UserTaskEntity>? Tasks { get; set; }

    public required RoleEntity Role { get; set; }
}
