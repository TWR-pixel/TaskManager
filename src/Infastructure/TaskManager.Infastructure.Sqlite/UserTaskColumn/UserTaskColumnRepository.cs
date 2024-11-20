﻿using Microsoft.EntityFrameworkCore;
using TaskManager.DALImplementation.Sqlite.Common;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.UseCases.TaskColumns;

namespace TaskManager.DALImplementation.Sqlite.UserTaskColumn;

public sealed class UserTaskColumnRepository(TaskManagerDbContext dbContext) : RepositoryBase<UserTaskColumnEntity>(dbContext), IUserTaskColumnRepository
{
    public async Task<UserTaskColumnEntity?> GetByIdWithOwnerAndUserTasksAsync(int id, CancellationToken cancellationToken = default)
    {
        var userTaskColumnEntity = await DbContext.TaskColumns
                .Include(u => u.Owner)
                .Include(u => u.TasksInColumn)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return userTaskColumnEntity;
    }

    public async Task<UserTaskColumnEntity?> GetByIdWithUserTasksAsync(int id, CancellationToken cancellationToken = default)
    {
        var userTaskColumnEntity = await DbContext.TaskColumns
                .Include(u => u.TasksInColumn)
           .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return userTaskColumnEntity;
    }
}