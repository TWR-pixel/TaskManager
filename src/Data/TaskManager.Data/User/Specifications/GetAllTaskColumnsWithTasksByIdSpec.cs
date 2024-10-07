using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetAllTaskColumnsWithTasksByIdSpec : Specification<UserEntity>
{
    public GetAllTaskColumnsWithTasksByIdSpec(int userId)
    {
        Query
            .Where(u => u.Id == userId)
            .Include(u => u.TaskColumns);
    }
}
