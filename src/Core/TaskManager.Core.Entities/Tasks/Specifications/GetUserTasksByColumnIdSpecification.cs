using Ardalis.Specification;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.Entities.Tasks.Specifications;

public sealed class GetUserTasksByColumnIdSpecification : SingleResultSpecification<TaskColumnEntity>
{
    public GetUserTasksByColumnIdSpecification(int taskColumnId)
    {
        Query
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
