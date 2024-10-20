using Ardalis.Specification;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.UseCases.TaskColumns.Specifications;

public sealed class ReadTaskColumnsByIdWithTasksSpecification : SingleResultSpecification<TaskColumnEntity>
{
    public ReadTaskColumnsByIdWithTasksSpecification(int taskColumnId)
    {
        Query
            .AsNoTracking()
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
