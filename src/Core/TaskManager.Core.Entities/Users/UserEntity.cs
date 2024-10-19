using System.Diagnostics.CodeAnalysis;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Entities.Users;

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
        IsEmailConfirmed = isEmailConfirmed;
    }

    public UserEntity() { }

    public required string EmailLogin { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;

    public IEnumerable<TaskColumnEntity>? TaskColumns { get; set; }
    public IEnumerable<UserTaskEntity>? Tasks { get; set; }

    public required RoleEntity Role { get; set; }
}
