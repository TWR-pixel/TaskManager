using Ardalis.Specification;

namespace TaskManager.Core.Entities.Users.Specifications;

public sealed class GetUserByRefreshToken : IncludeUserRoleSpecificationBase
{
    public GetUserByRefreshToken(string refreshToken)
    {
        Query
            .Where(u => u.RefreshToken == refreshToken);
    }
}
