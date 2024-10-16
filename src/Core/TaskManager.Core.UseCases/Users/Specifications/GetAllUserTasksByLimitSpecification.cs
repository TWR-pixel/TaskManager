using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Users.Specifications;

public sealed class GetAllUserTasksByLimitSpecification : Specification<UserEntity>
{
    public GetAllUserTasksByLimitSpecification(int ownerId)
    {
        //Query
        //    .Include(q => q.Tasks)
    }
}
