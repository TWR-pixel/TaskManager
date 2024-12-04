using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Role;
using TaskManager.Application.UserOrganization;
using TaskManager.Application.UserTask;
using TaskManager.Application.UserTaskColumn;

namespace TaskManager.Application.User;

public sealed record UserDto
{
    [SetsRequiredMembers]
    public UserDto(string? emailLogin,
                   string? username,
                   IEnumerable<UserTaskColumnDto>? taskColumns,
                   IEnumerable<UserTaskDto>? tasks,
                   IEnumerable<UserOrganizationResponse>? userOrganizations,
                   RoleDto role,
                   string? profileImageLink,
                   DateTime registeredAt,
                   DateTime lastLoginAt,
                   DateTime passwordUpdatedAt)
    {
        EmailLogin = emailLogin;
        Username = username;
        TaskColumns = taskColumns;
        Tasks = tasks;
        Role = role;
        ProfileImageLink = profileImageLink;
        RegisteredAt = registeredAt;
        LastLoginAt = lastLoginAt;
        PasswordUpdatedAt = passwordUpdatedAt;
    }

    public required string? EmailLogin { get; set; }
    public required string? Username { get; set; }
    public string? ProfileImageLink { get; set; }

    public DateTime RegisteredAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public DateTime PasswordUpdatedAt { get; set; }

    public IEnumerable<UserTaskColumnDto>? TaskColumns { get; set; }
    public IEnumerable<UserTaskDto>? Tasks { get; set; }
    public IEnumerable<UserOrganizationResponse>? UserOrganizations { get; set; }

    public required RoleDto Role { get; set; }
}
