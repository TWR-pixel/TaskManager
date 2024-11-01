using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Role;
using TaskManager.Application.TaskColumns;
using TaskManager.Application.Tasks.Requests;

namespace TaskManager.Application.Users;

public sealed record UserDto
{
    [SetsRequiredMembers]
    public UserDto(string emailLogin,
                   string username,
                   string passwordHash,
                   string passwordSalt,
                   string refreshToken,
                   IEnumerable<UserTaskColumnDto>? taskColumns,
                   IEnumerable<UserTaskDto>? tasks,
                   RoleDto role)
    {
        EmailLogin = emailLogin;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        RefreshToken = refreshToken;
        TaskColumns = taskColumns;
        Tasks = tasks;
        Role = role;
    }

    public required string EmailLogin { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required string RefreshToken { get; set; }

    public IEnumerable<UserTaskColumnDto>? TaskColumns { get; set; }
    public IEnumerable<UserTaskDto>? Tasks { get; set; }

    public required RoleDto Role { get; set; }
}
