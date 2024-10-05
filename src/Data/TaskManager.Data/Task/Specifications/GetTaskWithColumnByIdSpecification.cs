using Ardalis.Specification;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Data.Task.Specifications;

public sealed class GetTaskWithColumnByIdSpecification : SingleResultSpecification<TaskEntity>
{
    public GetTaskWithColumnByIdSpecification(int taskId)
    {
        Query
            .Where(t => t.Id == taskId)
            .Include(t => t.TaskColumn);
    }
}
