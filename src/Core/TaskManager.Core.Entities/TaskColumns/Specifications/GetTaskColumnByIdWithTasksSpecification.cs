using Ardalis.Specification;    

namespace TaskManager.Core.Entities.TaskColumns.Specifications;

public sealed class GetTaskColumnByIdWithTasksSpecification : SingleResultSpecification<TaskColumnEntity>
{
    public GetTaskColumnByIdWithTasksSpecification(int taskColumnId)
    {
        Query
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
