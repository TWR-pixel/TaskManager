using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserByEmailLoginSpecification : SingleResultSpecification<UserEntity>
{
    public GetUserByEmailLoginSpecification(string emailLogin)
    {
        Query
            .Where(u => u.EmailLogin == emailLogin)
            .Include(u => u.Role);
    }
}
