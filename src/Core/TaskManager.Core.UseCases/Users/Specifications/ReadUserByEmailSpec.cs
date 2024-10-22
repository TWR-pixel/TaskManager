using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Users.Specifications;

public sealed class ReadUserByEmailSpec : SingleResultSpecification<UserEntity>
{
    public ReadUserByEmailSpec(string emailLogin)
    {
        Query
            .AsNoTracking()
            .Where(u => u.EmailLogin == emailLogin)
                .Include(u => u.Role);
    }
}
