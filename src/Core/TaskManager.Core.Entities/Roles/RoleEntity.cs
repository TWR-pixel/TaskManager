using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.Entities.Roles;

public sealed class RoleEntity :IdentityRole<int>, IEntity
{

    public RoleEntity() { }


    [SetsRequiredMembers]
    public RoleEntity(string name)
    {
        Name = name;
    }

}
