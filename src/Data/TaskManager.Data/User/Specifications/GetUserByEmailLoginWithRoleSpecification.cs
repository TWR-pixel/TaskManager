using Ardalis.Specification;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserByEmailLoginWithRoleSpecification : IncludeUserRoleSpecificationBase
{
    public GetUserByEmailLoginWithRoleSpecification(string emailLogin)
    {
        Query
            .Where(u => u.EmailLogin == emailLogin);
    }
}
