using Ardalis.Specification;
using TaskManager.Domain.Entities.Roles;

namespace TaskManager.Domain.UseCases.Roles.Specifications;

public sealed class GetRoleByNameSpec : SingleResultSpecification<RoleEntity>
{
    public GetRoleByNameSpec(string roleName)
    {
        Query
            .Where(r => r.Name.ToUpper() == roleName.ToUpper());
    }
}
