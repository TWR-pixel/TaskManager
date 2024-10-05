using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserWithTasksByIdSpecification : SingleResultSpecification<UserEntity>
{
    public GetUserWithTasksByIdSpecification(int userId)
    {
        Query
            .Where(t => t.Id == userId)
            .Include(u => u.Tasks);

    }
}
