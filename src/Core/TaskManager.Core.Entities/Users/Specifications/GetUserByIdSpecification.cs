using Ardalis.Specification;

namespace TaskManager.Core.Entities.Users.Specifications;

public sealed class GetUserByIdSpecification : SingleResultSpecification<UserEntity>
{
    public GetUserByIdSpecification(int id)
    {
        Query
            .Where(u => u.Id == id)
            .Include(u => u.Role);
    }
}
