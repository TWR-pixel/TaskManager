using Ardalis.Specification;
using TaskManager.Core.Entities.Roles;

namespace TaskManager.Core.UseCases.Roles.Specifications;

public sealed class ReadRoleByNameSpecification : SingleResultSpecification<RoleEntity>
{
    public ReadRoleByNameSpecification(string roleName)
    {
        Query
            .AsNoTracking()
            .Where(r => r.Name.ToUpper() == roleName.ToUpper());
    }
}
