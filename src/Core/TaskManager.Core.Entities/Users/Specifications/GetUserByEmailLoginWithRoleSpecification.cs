using Ardalis.Specification;

namespace TaskManager.Core.Entities.Users.Specifications;

public sealed class GetUserByEmailLoginWithRoleSpecification : IncludeUserRoleSpecificationBase
{
    public GetUserByEmailLoginWithRoleSpecification(string emailLogin)
    {
        Query
            .Where(u => u.EmailLogin == emailLogin);
    }
}
