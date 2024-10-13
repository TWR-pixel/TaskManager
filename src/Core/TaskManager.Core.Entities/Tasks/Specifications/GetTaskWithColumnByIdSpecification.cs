using Ardalis.Specification;

namespace TaskManager.Core.Entities.Tasks.Specifications;

public sealed class GetTaskWithColumnByIdSpecification : SingleResultSpecification<UserTaskEntity>
{
    public GetTaskWithColumnByIdSpecification(int taskId)
    {
        Query
            .Where(t => t.Id == taskId)
            .Include(t => t.TaskColumn);
    }
}
