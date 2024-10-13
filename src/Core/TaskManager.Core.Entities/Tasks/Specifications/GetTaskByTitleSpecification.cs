using Ardalis.Specification;
namespace TaskManager.Core.Entities.Tasks.Specifications;

public sealed class GetTaskByTitleSpecification : SingleResultSpecification<UserTaskEntity>
{
    private readonly string taskTitle;

    public GetTaskByTitleSpecification(string taskTitle)
    {
        this.taskTitle = taskTitle;
    }

}
