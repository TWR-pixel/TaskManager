using Ardalis.Specification;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Data.TaskColumn.Specifications;

public sealed class GetTaskColumnByIdWithTasksSpecification : SingleResultSpecification<TaskColumnEntity>
{
    public GetTaskColumnByIdWithTasksSpecification(int taskColumnId)
    {
        Query
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
