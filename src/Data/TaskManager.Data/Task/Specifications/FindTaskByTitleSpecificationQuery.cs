using Ardalis.Specification;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Data.Task.Specifications;

public sealed class FindTaskByTitleSpecificationQuery : SingleResultSpecification<TaskEntity>
{
    public FindTaskByTitleSpecificationQuery(string taskTitle)
    {
        Query
            .Where(t => t.Title.ToLower() == taskTitle.ToLower());
    }
}
