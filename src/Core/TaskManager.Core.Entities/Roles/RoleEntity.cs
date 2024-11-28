using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.Entities.Roles;

public sealed class RoleEntity : EntityBase
{

    [MinLength(3)]
    public required string Name { get; set; }

    public RoleEntity() { }


    [SetsRequiredMembers]
    public RoleEntity(string name)
    {
        Name = name;
    }

}
