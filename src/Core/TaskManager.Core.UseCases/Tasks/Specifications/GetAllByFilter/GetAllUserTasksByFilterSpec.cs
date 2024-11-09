using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Domain.UseCases.Tasks.Specifications.GetAllByFilter;

public sealed class GetAllUserTasksByFilterSpec : SingleResultSpecification<UserEntity>
{
    public GetAllUserTasksByFilterSpec()
    {

    }
}
