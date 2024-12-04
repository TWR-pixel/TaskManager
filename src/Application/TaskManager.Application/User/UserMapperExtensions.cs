using TaskManager.Application.Role;
using TaskManager.Application.User;
using TaskManager.Application.UserOrganization;
using TaskManager.Application.UserTask;
using TaskManager.Application.UserTaskColumn;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Application.User;

public static class UserMapperExtensions
{
    public static UserDto ToResponse(this UserEntity entity)
    {
        entity.UserTaskColumns ??= [];
        entity.UserTasks ??= [];
        entity.UserOrganizations ??= [];

        var dto = new UserDto(entity.EmailLogin,
                              entity.UserName,
                              entity.UserTaskColumns.ToResponses(),
                              entity.UserTasks.ToResponses(),
                              entity.UserOrganizations.ToResponses(),
                              entity.Role.ToResponse(),
                              entity.ProfileImageLink,
                              entity.RegisteredAt,
                              entity.LastLoginAt,
                              entity.PasswordUpdatedAt);

        return dto;
    }
}
