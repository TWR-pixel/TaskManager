using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.Entities.Roles;

[Table("roles")]
[Index(nameof(Name))]
public sealed class RoleEntity : EntityBase
{

    [MinLength(3)]
    [Column("name")]
    public required string Name { get; set; }

    public RoleEntity() { }


    [SetsRequiredMembers]
    public RoleEntity(string name)
    {
        Name = name;
    }

}
