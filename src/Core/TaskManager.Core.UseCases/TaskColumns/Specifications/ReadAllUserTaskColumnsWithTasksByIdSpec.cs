using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.TaskColumns.Specifications;

public sealed class ReadAllUserTaskColumnsWithTasksByIdSpec : SingleResultSpecification<UserEntity>
{
    /// <summary>
    /// Returns all columns with tasks by user id. No tracking
    /// </summary>
    /// <param name="userId">User id</param>
    public ReadAllUserTaskColumnsWithTasksByIdSpec(int userId)
    {
        Query
            .AsNoTracking()
            .Where(u => u.Id == userId)
              .Include(u => u.TaskColumns)!
                    .ThenInclude(t => t.TasksInColumn);
    }

}
