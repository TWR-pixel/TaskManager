using Ardalis.Specification;
using TaskManager.Domain.Entities.Tasks;

namespace TaskManager.Domain.UseCases.Tasks.Specifications;

public sealed class ReadTaskByTitleSpecification : SingleResultSpecification<UserTaskEntity>
{
    public ReadTaskByTitleSpecification(string taskTitle)
    {
        Query
            .AsNoTracking()
            .Where(t => t.Title == taskTitle);
    }

}
