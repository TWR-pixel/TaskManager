using TaskManager.Core.Entities.Common.Repositories;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.Common.UnitOfWorks;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    public IRepositoryBaseCore<UserTaskEntity> UserTasks { get; init; }
    public IRepositoryBaseCore<TaskColumnEntity> UserTaskColumns { get; init; }
    public IRepositoryBaseCore<RoleEntity> Roles { get; init; }
    public IRepositoryBaseCore<UserEntity> Users { get; init; }
}
