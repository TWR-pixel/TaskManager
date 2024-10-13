using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.Users.Specifications;

public abstract class IncludeUserRoleSpecificationBase : SingleResultSpecification<UserEntity>
{
    protected IncludeUserRoleSpecificationBase()
    {
        Query.Include(u => u.Role);
    }
}
