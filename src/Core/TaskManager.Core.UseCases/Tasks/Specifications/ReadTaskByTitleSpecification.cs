using Ardalis.Specification;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.UseCases.Tasks.Specifications;

public sealed class ReadTaskByTitleSpecification : SingleResultSpecification<UserTaskEntity>
{
    public ReadTaskByTitleSpecification(string taskTitle)
    {
        Query
            .AsNoTracking()
            .Where(t => t.Title == taskTitle);
    }

}
