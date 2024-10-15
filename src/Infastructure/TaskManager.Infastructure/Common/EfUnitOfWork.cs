using TaskManager.Core.Entities.Common.Repositories;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Infastructure.Sqlite.Common;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;

    public IRepositoryBaseCore<UserTaskEntity> UserTasks { get; init; }
    public IRepositoryBaseCore<TaskColumnEntity> UserTaskColumns { get; init; }
    public IRepositoryBaseCore<RoleEntity> Roles { get; init; }
    public IRepositoryBaseCore<UserEntity> Users { get; init; }

    public EfUnitOfWork(TaskManagerDbContext dbContext,
                        IRepositoryBaseCore<UserTaskEntity> userTasks,
                        IRepositoryBaseCore<TaskColumnEntity> userTaskColumns,
                        IRepositoryBaseCore<RoleEntity> roles,
                        IRepositoryBaseCore<UserEntity> users)
    {
        _dbContext = dbContext;
        UserTasks = userTasks;
        UserTaskColumns = userTaskColumns;
        Roles = roles;
        Users = users;
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
