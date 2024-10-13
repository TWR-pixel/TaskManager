using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.Tasks.Specifications;

public sealed class GetAllUserTasksByIdSpecification : SingleResultSpecification<UserEntity>
{
    /// <summary>
    /// Returns new user with his tasks
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTasks"></param>
    /// <param name="includeTaskColumns"></param>
    public GetAllUserTasksByIdSpecification(int userId)
    {
        Query.Where(t => t.Id == userId)
            .Include(t => t.Tasks);
    }
}
