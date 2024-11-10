using Ardalis.Specification;
using TaskManager.Domain.Entities.TaskColumns;

namespace TaskManager.Domain.UseCases.TaskColumns.Specifications;

public sealed class ReadTaskColumnsByIdWithOwnerSpec : SingleResultSpecification<UserTaskColumnEntity>
{
    public ReadTaskColumnsByIdWithOwnerSpec(int taskColumnId)
    {
        Query
            .AsNoTracking()
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.Owner)
            .Include(t => t.TasksInColumn);
    }
}
