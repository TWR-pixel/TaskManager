using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Role;
using TaskManager.Application.TaskColumns;
using TaskManager.Application.Tasks;

namespace TaskManager.Application.Users;

public sealed record UserDto
{
    [SetsRequiredMembers]
    public UserDto(string emailLogin,
                   string username,
                   IEnumerable<UserTaskColumnDto>? taskColumns,
                   IEnumerable<UserTaskDto>? tasks,
                   RoleDto role)
    {
        EmailLogin = emailLogin;
        Username = username;
        TaskColumns = taskColumns;
        Tasks = tasks;
        Role = role;
    }

    public required string EmailLogin { get; set; }
    public required string Username { get; set; }

    public IEnumerable<UserTaskColumnDto>? TaskColumns { get; set; }
    public IEnumerable<UserTaskDto>? Tasks { get; set; }

    public required RoleDto Role { get; set; }
}
