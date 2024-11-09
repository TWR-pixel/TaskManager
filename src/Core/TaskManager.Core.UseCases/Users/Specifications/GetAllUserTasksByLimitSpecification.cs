using Ardalis.Specification;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.UseCases.Users.Specifications;

public sealed class GetAllUserTasksByLimitSpecification : Specification<UserEntity>
{
    public GetAllUserTasksByLimitSpecification(int ownerId)
    {
        //Query
        //    .Include(q => q.Tasks)
    }
}
