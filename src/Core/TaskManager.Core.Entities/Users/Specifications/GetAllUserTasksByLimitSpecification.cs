using Ardalis.Specification;

namespace TaskManager.Core.Entities.Users.Specifications;

public sealed class GetAllUserTasksByLimitSpecification : Specification<UserEntity>
{
    public GetAllUserTasksByLimitSpecification(int ownerId)
    {
        //Query
        //    .Include(q => q.Tasks)
    }
}
