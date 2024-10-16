using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.UseCases.Users.Specifications;

public sealed class ReadUserByRefreshToken : SingleResultSpecification<UserEntity>
{
    public ReadUserByRefreshToken(string refreshToken)
    {
        Query
            .AsNoTracking()
            .Where(u => u.RefreshToken == refreshToken)
            .Include(u => u.Role);
    }
}
