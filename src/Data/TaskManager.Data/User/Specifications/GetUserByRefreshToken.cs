using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserByRefreshToken : SingleResultSpecification<UserEntity>
{
    public GetUserByRefreshToken(string refreshToken)
    {
        Query
            .Where(u => u.RefreshToken == refreshToken)
            .Include(u => u.Role);
    }
}
