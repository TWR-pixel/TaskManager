using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Entities.Users;

[Table("users")]
[Index(nameof(EmailLogin), nameof(RegisteredAt))]
public sealed class UserEntity : EntityBase
{
    [SetsRequiredMembers]
    public UserEntity(RoleEntity role,
                      string emailLogin,
                      string username,
                      string passwordHash,
                      string passwordSalt,
                      bool isEmailConfirmed = false)
    {
        Role = role;
        EmailLogin = emailLogin;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        IsEmailVerified = isEmailConfirmed;
    }

    public UserEntity() { }

    [Column("email_login")]
    [StringLength(128, MinimumLength = 3)]
    [EmailAddress]
    public required string EmailLogin { get; set; }

    [Column("username")]
    [StringLength(128, MinimumLength = 3)]
    public required string Username { get; set; }


    [Column("password_hash")]
    [StringLength(256, MinimumLength = 3)]
    public required string PasswordHash { get; set; }

    [Column("password_salt")]
    [StringLength(256, MinimumLength = 3)]
    public required string PasswordSalt { get; set; }


    [Column("registered_at")]
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    [Column("password_updated_at")]
    public DateTime PasswordUpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("is_email_verified")]
    public bool IsEmailVerified { get; set; } = true;

    public IEnumerable<TaskColumnEntity>? TaskColumns { get; set; }
    public IEnumerable<UserTaskEntity>? Tasks { get; set; }

    [ForeignKey("role_id")]
    public required RoleEntity Role { get; set; }
}
