using Ardalis.Specification;
using TaskManager.Core.Entities.Roles;

namespace TaskManager.Core.UseCases.Roles.Specifications;

public sealed class GetRoleByNameSpec : SingleResultSpecification<RoleEntity>
{
    public GetRoleByNameSpec(string roleName)
    {
        Query
            .Where(r => r.Name.ToUpper() == roleName.ToUpper());
    }
}
