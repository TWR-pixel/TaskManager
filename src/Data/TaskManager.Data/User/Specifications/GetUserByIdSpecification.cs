using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetUserByIdSpecification : SingleResultSpecification<UserEntity>
{
    public GetUserByIdSpecification(int id)
    { 
        Query.Where(u => u.Id == id);
    }
}
