using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.Role.Specifications;

public sealed class GetRoleByNameSpecification : SingleResultSpecification<RoleEntity>
{
    public GetRoleByNameSpecification(string roleName)
    {
        Query
            .Where(r => r.Name.ToLower() == roleName.ToLower());
    }
}
