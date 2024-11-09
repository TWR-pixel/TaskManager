using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Domain.UseCases.Users.Specifications;

public sealed class GetAllUserTasksByFilterSpec : SingleResultSpecification<UserEntity>
{
    public GetAllUserTasksByFilterSpec(string ordering = "asc")
    {

    }
}
