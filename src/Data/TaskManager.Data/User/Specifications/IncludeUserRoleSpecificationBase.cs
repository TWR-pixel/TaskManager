using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public abstract class IncludeUserRoleSpecificationBase : Specification<UserEntity>
{
    protected IncludeUserRoleSpecificationBase()
    {
        Query.Include(u => u.Role);
    }
}
