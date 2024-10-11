using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.TaskColumn.Specifications;

public sealed class GetAllTaskColumnsWithTasksByIdSpec : SingleResultSpecification<UserEntity>
{
    public GetAllTaskColumnsWithTasksByIdSpec(int userId)
    {
        Query
            .Where(u => u.Id == userId)
            .Include(u => u.TaskColumns);
    }
}
