using TaskManager.Application.Role;
using TaskManager.Application.TaskColumns;
using TaskManager.Application.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.Users;

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
