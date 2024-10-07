using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetAllUserTasksByLimitSpecification : Specification<UserEntity>
{
    public GetAllUserTasksByLimitSpecification(int ownerId)
    {
        //Query
        //    .Include(q => q.Tasks)
    }
}
