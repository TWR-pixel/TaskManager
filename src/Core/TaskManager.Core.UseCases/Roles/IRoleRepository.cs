using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.Roles;

public interface IRoleRepository : IRepository<RoleEntity>
{
    public Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
