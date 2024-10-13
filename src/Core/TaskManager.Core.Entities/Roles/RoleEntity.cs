using System.Diagnostics.CodeAnalysis;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Core.Entities.Roles;

public sealed class RoleEntity : EntityBase
{
    public required string Name { get; set; }

    public RoleEntity()
    {

    }

    [SetsRequiredMembers]
    public RoleEntity(string name)
    {
        Name = name;
    }
}
