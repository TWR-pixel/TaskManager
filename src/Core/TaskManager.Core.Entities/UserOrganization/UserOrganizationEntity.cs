using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.Entities.UserOrganization;

public class UserOrganizationEntity : IEntity
{
    [SetsRequiredMembers]
    public UserOrganizationEntity(string name, UserEntity owner)
    {
        Name = name;
        Owner = owner;
    }

    public UserOrganizationEntity()
    {
        
    }

    public int Id { get; set; }
    public required string Name { get; set; }

    public required UserEntity Owner { get; set; }

}
