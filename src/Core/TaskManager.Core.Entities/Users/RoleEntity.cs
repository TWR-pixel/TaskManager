using TaskManager.Core.Entities.Common;

namespace TaskManager.Core.Entities.Users;

public class RoleEntity : EntityBase
{
    public required string Name { get; set; }

    public RoleEntity()
    {
        
    }
}
