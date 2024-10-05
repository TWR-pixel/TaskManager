using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserEntityByEmailLoginSpecification : SingleResultSpecification<UserEntity>
{
    public GetUserEntityByEmailLoginSpecification(string emailLogin)
    {
        Query
            .Where(u => u.EmailLogin == emailLogin);
    }
}
