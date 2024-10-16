using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Users.Specifications;

public sealed class ReadUserByIdSpecification : SingleResultSpecification<UserEntity>
{
    public ReadUserByIdSpecification(int id)
    {
        Query
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Include(u => u.Role);
    }
}
