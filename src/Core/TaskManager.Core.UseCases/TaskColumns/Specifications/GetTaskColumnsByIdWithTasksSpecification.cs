using Ardalis.Specification;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.UseCases.TaskColumns.Specifications;

public sealed class GetTaskColumnsByIdWithTasksSpecification : SingleResultSpecification<TaskColumnEntity>
{
    public GetTaskColumnsByIdWithTasksSpecification(int taskColumnId)
    {
        Query
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
