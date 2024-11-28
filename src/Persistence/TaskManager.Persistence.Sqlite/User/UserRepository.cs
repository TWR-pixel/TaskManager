using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Users;
using TaskManager.Persistence.Sqlite.Common;

namespace TaskManager.Persistence.Sqlite.User;

public sealed class UserRepository(TaskManagerDbContext dbContext) : RepositoryBase<UserEntity>(dbContext), IUserRepository
{
    public async Task<UserEntity?> GetAllUserTaskColumnsWithTasksByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userEntity = await DbContext.Users
            .AsNoTracking()
              .Include(u => u.TaskColumns)!
                    .ThenInclude(t => t.TasksInColumn)
              .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return userEntity;
    }

    public async Task<UserEntity?> GetAllUserTasks(int id, CancellationToken cancellationToken = default)
    {
        var userEntity = await DbContext.Users
            .AsNoTracking()
            .Include(u => u.Tasks)!
                .ThenInclude(u => u.TaskColumn)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return userEntity;
    }

    public async Task<UserEntity?> GetAllUserTasksByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userEntity = await DbContext.Users
                    .AsNoTracking()
                        .Include(t => t.Tasks)!
                            .ThenInclude(t => t.TaskColumn)
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return userEntity;
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var userEntity = await DbContext.Users
                .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.EmailLogin == email, cancellationToken);

        return userEntity;
    }

    public async Task<UserEntity?> GetWithRoleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var userEntity = await DbContext.Users
                .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return userEntity;
    }
}
