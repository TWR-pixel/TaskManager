using Microsoft.EntityFrameworkCore;
using TaskManager.DALImplementation.Sqlite.Common;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.UseCases.Roles;

namespace TaskManager.DALImplementation.Sqlite.Role;

public sealed class RoleRepository(TaskManagerDbContext dbContext) : RepositoryBase<RoleEntity>(dbContext), IRoleRepository
{
    public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var roleEntity = await DbContext.UserRoles
            .FirstOrDefaultAsync(r => r.Name.ToUpper() == name.ToUpper(), cancellationToken);

        return roleEntity;
    }
}
