using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Users.Specifications;

public sealed class GetUserByEmailSpec : SingleResultSpecification<UserEntity>
{
    public GetUserByEmailSpec(string emailLogin)
    {
        Query
            .Where(u => u.EmailLogin == emailLogin)
                .Include(u => u.Role);
        
    }
}
