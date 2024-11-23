﻿using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Infrastructure.Sqlite.Common;

namespace TaskManager.Infrastructure.Sqlite.Role;

public sealed class RoleRepository(TaskManagerDbContext dbContext) : RepositoryBase<RoleEntity>(dbContext), IRoleRepository
{
    public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var roleEntity = await DbContext.UserRoles
            .FirstOrDefaultAsync(r => r.Name.ToUpper() == name.ToUpper(), cancellationToken);

        return roleEntity;
    }
}
