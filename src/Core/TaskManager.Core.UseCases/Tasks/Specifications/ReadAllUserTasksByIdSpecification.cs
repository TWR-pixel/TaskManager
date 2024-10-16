using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Tasks.Specifications;

public sealed class ReadAllUserTasksByIdSpecification : SingleResultSpecification<UserEntity>
{
    /// <summary>
    /// Returns new user with his tasks
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTasks"></param>
    /// <param name="includeTaskColumns"></param>
    public ReadAllUserTasksByIdSpecification(int userId)
    {
        Query
            .AsNoTracking()
            .Where(t => t.Id == userId)
            .Include(t => t.Tasks);
    }
}
