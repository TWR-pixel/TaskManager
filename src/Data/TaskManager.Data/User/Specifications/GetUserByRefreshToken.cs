using Ardalis.Specification;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserByRefreshToken : IncludeUserRoleSpecificationBase
{
    public GetUserByRefreshToken(string refreshToken)
    {
        Query
            .Where(u => u.RefreshToken == refreshToken);
    }
}
