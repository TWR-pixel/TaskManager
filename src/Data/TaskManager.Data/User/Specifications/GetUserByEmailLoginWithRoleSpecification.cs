using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserByEmailLoginWithRoleSpecification : SingleResultSpecification<UserEntity>
{
    public GetUserByEmailLoginWithRoleSpecification(string emailLogin)
    {
        Query
            .Where(u => u.EmailLogin == emailLogin)
            .Include(u => u.Role);
    }
}
