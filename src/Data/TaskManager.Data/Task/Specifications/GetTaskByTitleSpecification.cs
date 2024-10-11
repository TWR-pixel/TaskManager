using Ardalis.Specification;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Data.Task.Specifications;

public sealed class GetTaskByTitleSpecification : SingleResultSpecification<UserTaskEntity>
{
    public GetTaskByTitleSpecification(string taskTitle)
    {
        Query
            .Where(t => t.Title.ToUpper() == taskTitle.ToUpper());
    }
}
