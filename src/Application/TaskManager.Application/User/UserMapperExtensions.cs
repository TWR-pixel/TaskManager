using TaskManager.Application.Role;
using TaskManager.Application.User;
using TaskManager.Application.UserTask;
using TaskManager.Application.UserTaskColumn;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Application.User;

public static class UserMapperExtensions
{
    public static UserDto ToResponse(this UserEntity entity)
    {
        entity.TaskColumns ??= [];
        entity.Tasks ??= [];

        var dto = new UserDto(entity.EmailLogin,
                              entity.Username,
                              entity.TaskColumns.ToResponses(),
                              entity.Tasks.ToResponses(),
                              entity.Role.ToResponse());

        return dto;
    }
}
