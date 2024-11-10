using Ardalis.Specification;
using TaskManager.Domain.Entities.TaskColumns;

namespace TaskManager.Domain.UseCases.Tasks.Specifications;

public sealed class GetUserTasksByColumnIdSpecification : SingleResultSpecification<UserTaskColumnEntity>
{
    public GetUserTasksByColumnIdSpecification(int taskColumnId)
    {
        Query
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
