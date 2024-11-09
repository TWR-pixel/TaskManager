using Ardalis.Specification;
using TaskManager.Domain.Entities.Tasks;

namespace TaskManager.Domain.UseCases.Tasks.Specifications;

public sealed class GetTaskWithColumnByIdSpecification : SingleResultSpecification<UserTaskEntity>
{
    public GetTaskWithColumnByIdSpecification(int taskId)
    {
        Query
            .Where(t => t.Id == taskId)
            .Include(t => t.TaskColumn);
    }
}
