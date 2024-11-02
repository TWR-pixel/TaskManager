using TaskManager.Core.Entities.Roles;

namespace TaskManager.Application.Role;

public static class RoleMapperExtensions
{
    public static RoleDto ToResponse(this RoleEntity entity)
    {
        var dto = new RoleDto(entity.Name);

        return dto;
    }

    public static RoleEntity ToEntity(this RoleDto dto)
    {
        var entity = new RoleEntity(dto.Name);

        return entity;
    }
}
