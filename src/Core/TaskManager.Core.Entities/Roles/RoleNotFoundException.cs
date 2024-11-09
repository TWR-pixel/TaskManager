namespace TaskManager.Domain.Entities.Roles;

public class RoleNotFoundException(string roleName) : Exception($"Role with name '{roleName}' not found.")
{
}
