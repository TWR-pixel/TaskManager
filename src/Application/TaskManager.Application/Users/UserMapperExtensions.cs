using TaskManager.Application.Role;
using TaskManager.Application.TaskColumns;
using TaskManager.Application.Tasks.Requests;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.Users;

public static class UserMapperExtensions
{
    public static UserDto ToResponse(this UserEntity entity)
    {
        var dto = new UserDto(entity.EmailLogin,
                              entity.Username,
                              entity.PasswordHash,
                              entity.PasswordSalt,
                              entity.RefreshToken,
                              entity.TaskColumns!.ToResponses(),
                              entity.Tasks!.ToResponses(),
                              entity.Role.ToResponse());

        return dto;
    }
}
