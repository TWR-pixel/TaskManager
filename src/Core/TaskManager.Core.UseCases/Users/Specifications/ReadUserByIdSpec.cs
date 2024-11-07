using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Users.Specifications;

public sealed class ReadUserByIdSpec : SingleResultSpecification<UserEntity>
{
    public ReadUserByIdSpec(int id)
    {
        Query
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Include(u => u.Role);
    }
}
