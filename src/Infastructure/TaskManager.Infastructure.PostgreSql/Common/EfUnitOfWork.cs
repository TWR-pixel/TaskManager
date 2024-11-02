using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Common.Repositories;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infrastructure.PostgreSql.Common;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;

    public IRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IRepositoryBase<TaskColumnEntity> UserTaskColumns { get; init; }
    public IRepositoryBase<RoleEntity> Roles { get; init; }
    public IRepositoryBase<UserEntity> Users { get; init; }

    public EfUnitOfWork(TaskManagerDbContext dbContext,
                        IRepositoryBase<UserTaskEntity> userTasks,
                        IRepositoryBase<TaskColumnEntity> userTaskColumns,
                        IRepositoryBase<RoleEntity> roles,
                        IRepositoryBase<UserEntity> users)
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
